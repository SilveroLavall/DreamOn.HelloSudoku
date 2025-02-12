using System.Diagnostics;
using System.Numerics;
namespace DreamOn.HelloSudoku;
internal class RunProperties
{
    public int SkipInputLines { get; set; } = 0;
    public int TakeInputLines { get; set; } = 1000;
    public int DisplayInterval { get; set; } = 200;
}
internal class RunResult
{
    public int TotalInputLines { get; set; }
    public int LinesProcessed { get; set; }
    public Int64 CyclesGuesses { get; set; }
    public Int64 CyclesInvalidBranche { get; set; }
    public Stopwatch Stopwatch { get; set; } = new();
}
internal class IntervalResult
{
    public int Processed { get; set; }
    public TimeSpan Elapsed { get; set; }
    public double Avg_ms { get; set; }
    public Int64 CyclesGuesses { get; set; }
    public Int64 CyclesInvalidBranche { get; set; }
    public string Description { get; set; }
    public string MachineName { get; set; }
    public IntervalResult(RunResult runResult, SudokuResponse sudokuResponse)
    {
        Processed = runResult.LinesProcessed;
        Elapsed = runResult.Stopwatch.Elapsed;
        Avg_ms = Elapsed.TotalMilliseconds / Processed;
        CyclesGuesses = runResult.CyclesGuesses;
        CyclesInvalidBranche = runResult.CyclesInvalidBranche;
        Description = sudokuResponse.SudokuTactics;
        MachineName = System.Environment.MachineName;
    }
}
internal class SudokuEngine_20250130(RunProperties runProperties)
{
    public RunProperties RunProperties { get; set; } = runProperties;
    public Engine Engine { get; set; } = new();
    internal IntervalResult[] StartProcessingSudokuPuzzles()
    {
        RunResult RunResult = new();
        List<IntervalResult> IntervalResults = new();
        IntervalResult intervalResult = new(new(),new("", new(), new()));
        var inputLines = new SudokuPuzzles().GetSudokuPuzzles();
        RunResult.Stopwatch.Start();
        RunResult.TotalInputLines = inputLines.Length;
        var response = new SudokuResponse("", new(), new());
        foreach (var inputLine in inputLines.Skip(RunProperties.SkipInputLines).Take(RunProperties.TakeInputLines))
        {
            response = Engine.SolveSudoku(inputLine);
            RunResult.CyclesGuesses += response.SudokuSolution.CyclesGuesses;
            RunResult.CyclesInvalidBranche += response.SudokuSolution.CyclesInvalidBranche;

            if (++RunResult.LinesProcessed % RunProperties.DisplayInterval == 0)
            {
                intervalResult = new IntervalResult(RunResult, response);
                IntervalResults.Add(intervalResult);
                Console.WriteLine(intervalResult.ToJson());
            }
        }
        RunResult.Stopwatch.Stop();
        intervalResult = new IntervalResult(RunResult, response);
        IntervalResults.Add(intervalResult);
        Console.WriteLine(intervalResult.ToJson());
        return IntervalResults.ToArray();
    }
}
internal class Engine
{
    private bool Continue;
    private SudokuSolution SudokuSolution { get; set; } = new();
    public SudokuResponse SolveSudoku(string sudokuString)
    {
        var stopWatch = Stopwatch.StartNew();
        Continue = true;
        var sudokuRequest = new SudokuRequest(sudokuString);
        SudokuSolution = new();
        if (sudokuRequest.IsValid)
        {
            var neuralSudoku = sudokuRequest.SudokuPuzzle.PuzzleBinary.GetNeuralSudoku();
            SolveSudoku(neuralSudoku);
        }
        SudokuSolution.ElapsedTime = stopWatch.Elapsed;
        return new("First Neural", sudokuRequest, SudokuSolution);
    }
    private void SolveSudoku(short[] neuralSudoku)
    {
        if (!Continue) return;
        var index = neuralSudoku.GetSmartNextGuess(out short availableOptions);
        if (index > -1) // P0.No.More.Guesses
        {
            for (short m = 1; m <= 256; m <<= 1)
                if ((availableOptions & m) == m)
                {
                    ++SudokuSolution.CyclesGuesses;
                    var newNeuralSudoku = neuralSudoku.GetCopy();
                    newNeuralSudoku[index] = m;
                    newNeuralSudoku.EliminateOtherSudokuCellOptions(index, m);
                    SolveSudoku(newNeuralSudoku);
                }
        }
        else if (neuralSudoku.IsSudokuValid())
        {
            SudokuSolution.SolutionBinary = neuralSudoku;
            Continue = false;
        }
        else
        {
            ++SudokuSolution.CyclesInvalidBranche;
        }
    }
}
internal class SudokuRequest
{
    public string Input { get; set; } = string.Empty;
    public SudokuPuzzle SudokuPuzzle { get; set; } = new([]);
    public bool IsValid { get; set; }
    public SudokuRequest() { }
    public SudokuRequest(string sudokuString)
    {
        Input = sudokuString ?? string.Empty;
        SudokuPuzzle = new(Input.ConvertToSudokuBinary());
        IsValid = SudokuPuzzle.PuzzleBinary.Length == 81;
    }
}
internal class SudokuPuzzle(short[] puzzleBinary)
{
    public short[] PuzzleBinary { get; set; } = puzzleBinary;
    public int Clues { get; set; } = puzzleBinary.CountClues();
}
internal class SudokuSolution
{
    public short[] SolutionBinary { get; set; } = [];
    public int CyclesGuesses { get; set; }
    public int CyclesInvalidBranche { get; set; }
    public TimeSpan ElapsedTime { get; set; }
}
internal class SudokuResponse(string sudokuTactics, SudokuRequest sudokuRequest, SudokuSolution sudokuSolutions)
{
    public string SudokuTactics { get; set; } = sudokuTactics;
    public SudokuRequest SudokuRequest { get; set; } = sudokuRequest;
    public SudokuSolution SudokuSolution { get; set; } = sudokuSolutions;
}
internal static class SudokuExtensions
{
    internal static int CountClues(this short[] puzzleBinary)
    {
        return puzzleBinary.Count(c => BitOperations.PopCount((uint)c) == 1);
    }
    internal static short[] GetNeuralSudoku(this short[] puzzleBinary)
    {
        var neuralSudoku = new short[81];
        for (int i = 0; i < 81; i++)
            neuralSudoku[i] = 0b111111111;
        for (int i = 0; i < 81; i++)
        {
            if (puzzleBinary[i] == 0b0) continue;
            neuralSudoku[i] = puzzleBinary[i];
            neuralSudoku.EliminateOtherSudokuCellOptions(i, puzzleBinary[i]);
        }
        return neuralSudoku;
    }
    internal static void EliminateOtherSudokuCellOptions(this short[] neuralSudoku, int sudokuCellNumber, short sudokuCellValue)
    {
        var mappingToCells = SudokuDependencyMapping[sudokuCellNumber];
        for (int i = 0; i < 20; i++) // P1.Eliminate.Other.Cell.Option
        {
            int celnr = mappingToCells[i];
            var oldOptions = neuralSudoku[celnr];
            neuralSudoku[celnr] &= (short)~sudokuCellValue;
            if (neuralSudoku[celnr] == 0b0) return; // SR0.Invalid.Brance
            if (oldOptions != neuralSudoku[celnr] && BitOperations.PopCount((uint)neuralSudoku[celnr]) == 1) // SR1.NakedSingel
                neuralSudoku.EliminateOtherSudokuCellOptions(celnr, neuralSudoku[celnr]);
        }
    }
    internal static int GetSmartNextGuess(this short[] neuralSudoku, out short options)
    {
        var leastPopCount = 10;
        short optionsLeastPopCount = 0b0;
        var index = -1;
        for (int i = 0; i < 81; i++)
        {
            var popCount = BitOperations.PopCount((uint)neuralSudoku[i]);
            if (popCount < leastPopCount & popCount != 1)
            {
                leastPopCount = popCount;
                optionsLeastPopCount = neuralSudoku[i];
                index = i;
            }
        }
        options = optionsLeastPopCount;
        return index;
    }
    internal static short[] GetCopy(this short[] neuralSudoku)
    {
        var copy = new short[81];
        Array.Copy(neuralSudoku, copy, 81);
        return copy;
    }
    internal static bool IsSudokuValid(this short[] puzzleBinary)
    {
        for (int i = 0; i < 81; i++)
        {
            var sudokuCellValue = puzzleBinary[i];
            if (sudokuCellValue == 0b0) continue;
            var mapping = SudokuDependencyMapping[i];
            for (int j = 0; j < 20; j++)
            {
                if ((puzzleBinary[mapping[j]] & sudokuCellValue) > 0) return false;
            }
        }
        return true;
    }
    internal static short[] ConvertToSudokuBinary(this string sudokuString)
    {
        return sudokuString
            .Select(s => Convert.ToInt32(s) - 48)
            .Where(w => w >= 0 && w <= 9)
            .Select(s => IntToBin[s])
            .Take(81)
            .ToArray();
    }
    internal static string ToJson(this RunProperties runProperties)
    {
        return System.Text.Json.JsonSerializer.Serialize(runProperties);
    }
    internal static string ToJson(this IntervalResult intervalResult)
    {
        return System.Text.Json.JsonSerializer.Serialize(intervalResult);
    }
    internal static string ToJson(this IntervalResult[] intervalResults)
    {
        return System.Text.Json.JsonSerializer.Serialize(intervalResults);
    }
    private static readonly short[] IntToBin = [0, 1, 2, 4, 8, 16, 32, 64, 128, 256];
    private static readonly int[][] SudokuDependencyMapping =
    [
        [1, 2, 3, 4, 5, 6, 7, 8, 9, 18, 27, 36, 45, 54, 63, 72, 10, 11, 19, 20],
        [0, 2, 3, 4, 5, 6, 7, 8, 10, 19, 28, 37, 46, 55, 64, 73, 9, 11, 18, 20],
        [0, 1, 3, 4, 5, 6, 7, 8, 11, 20, 29, 38, 47, 56, 65, 74, 9, 10, 18, 19],
        [0, 1, 2, 4, 5, 6, 7, 8, 12, 21, 30, 39, 48, 57, 66, 75, 13, 14, 22, 23],
        [0, 1, 2, 3, 5, 6, 7, 8, 13, 22, 31, 40, 49, 58, 67, 76, 12, 14, 21, 23],
        [0, 1, 2, 3, 4, 6, 7, 8, 14, 23, 32, 41, 50, 59, 68, 77, 12, 13, 21, 22],
        [0, 1, 2, 3, 4, 5, 7, 8, 15, 24, 33, 42, 51, 60, 69, 78, 16, 17, 25, 26],
        [0, 1, 2, 3, 4, 5, 6, 8, 16, 25, 34, 43, 52, 61, 70, 79, 15, 17, 24, 26],
        [0, 1, 2, 3, 4, 5, 6, 7, 17, 26, 35, 44, 53, 62, 71, 80, 15, 16, 24, 25],

        [10, 11, 12, 13, 14, 15, 16, 17, 0, 18, 27, 36, 45, 54, 63, 72, 1, 2, 19, 20],
        [9, 11, 12, 13, 14, 15, 16, 17, 1, 19, 28, 37, 46, 55, 64, 73, 0, 2, 18, 20],
        [9, 10, 12, 13, 14, 15, 16, 17, 2, 20, 29, 38, 47, 56, 65, 74, 0, 1, 18, 19],
        [9, 10, 11, 13, 14, 15, 16, 17, 3, 21, 30, 39, 48, 57, 66, 75, 4, 5, 22, 23],
        [9, 10, 11, 12, 14, 15, 16, 17, 4, 22, 31, 40, 49, 58, 67, 76, 3, 5, 21, 23],
        [9, 10, 11, 12, 13, 15, 16, 17, 5, 23, 32, 41, 50, 59, 68, 77, 3, 4, 21, 22],
        [9, 10, 11, 12, 13, 14, 16, 17, 6, 24, 33, 42, 51, 60, 69, 78, 7, 8, 25, 26],
        [9, 10, 11, 12, 13, 14, 15, 17, 7, 25, 34, 43, 52, 61, 70, 79, 6, 8, 24, 26],
        [9, 10, 11, 12, 13, 14, 15, 16, 8, 26, 35, 44, 53, 62, 71, 80, 6, 7, 24, 25],

        [19, 20, 21, 22, 23, 24, 25, 26, 0, 9, 27, 36, 45, 54, 63, 72, 1, 2, 10, 11],
        [18, 20, 21, 22, 23, 24, 25, 26, 1, 10, 28, 37, 46, 55, 64, 73, 0, 2, 9, 11],
        [18, 19, 21, 22, 23, 24, 25, 26, 2, 11, 29, 38, 47, 56, 65, 74, 0, 1, 9, 10],
        [18, 19, 20, 22, 23, 24, 25, 26, 3, 12, 30, 39, 48, 57, 66, 75, 4, 5, 13, 14],
        [18, 19, 20, 21, 23, 24, 25, 26, 4, 13, 31, 40, 49, 58, 67, 76, 3, 5, 12, 14],
        [18, 19, 20, 21, 22, 24, 25, 26, 5, 14, 32, 41, 50, 59, 68, 77, 3, 4, 12, 13],
        [18, 19, 20, 21, 22, 23, 25, 26, 6, 15, 33, 42, 51, 60, 69, 78, 7, 8, 16, 17],
        [18, 19, 20, 21, 22, 23, 24, 26, 7, 16, 34, 43, 52, 61, 70, 79, 6, 8, 15, 17],
        [18, 19, 20, 21, 22, 23, 24, 25, 8, 17, 35, 44, 53, 62, 71, 80, 6, 7, 15, 16],

        [28, 29, 30, 31, 32, 33, 34, 35, 0, 9, 18, 36, 45, 54, 63, 72, 37, 38, 46, 47],
        [27, 29, 30, 31, 32, 33, 34, 35, 1, 10, 19, 37, 46, 55, 64, 73, 36, 38, 45, 47],
        [27, 28, 30, 31, 32, 33, 34, 35, 2, 11, 20, 38, 47, 56, 65, 74, 36, 37, 45, 46],
        [27, 28, 29, 31, 32, 33, 34, 35, 3, 12, 21, 39, 48, 57, 66, 75, 40, 41, 49, 50],
        [27, 28, 29, 30, 32, 33, 34, 35, 4, 13, 22, 40, 49, 58, 67, 76, 39, 41, 48, 50],
        [27, 28, 29, 30, 31, 33, 34, 35, 5, 14, 23, 41, 50, 59, 68, 77, 39, 40, 48, 49],
        [27, 28, 29, 30, 31, 32, 34, 35, 6, 15, 24, 42, 51, 60, 69, 78, 43, 44, 52, 53],
        [27, 28, 29, 30, 31, 32, 33, 35, 7, 16, 25, 43, 52, 61, 70, 79, 42, 44, 51, 53],
        [27, 28, 29, 30, 31, 32, 33, 34, 8, 17, 26, 44, 53, 62, 71, 80, 42, 43, 51, 52],

        [37, 38, 39, 40, 41, 42, 43, 44, 0, 9, 18, 27, 45, 54, 63, 72, 28, 29, 46, 47],
        [36, 38, 39, 40, 41, 42, 43, 44, 1, 10, 19, 28, 46, 55, 64, 73, 27, 29, 45, 47],
        [36, 37, 39, 40, 41, 42, 43, 44, 2, 11, 20, 29, 47, 56, 65, 74, 27, 28, 45, 46],
        [36, 37, 38, 40, 41, 42, 43, 44, 3, 12, 21, 30, 48, 57, 66, 75, 31, 32, 49, 50],
        [36, 37, 38, 39, 41, 42, 43, 44, 4, 13, 22, 31, 49, 58, 67, 76, 30, 32, 48, 50],
        [36, 37, 38, 39, 40, 42, 43, 44, 5, 14, 23, 32, 50, 59, 68, 77, 30, 31, 48, 49],
        [36, 37, 38, 39, 40, 41, 43, 44, 6, 15, 24, 33, 51, 60, 69, 78, 34, 35, 52, 53],
        [36, 37, 38, 39, 40, 41, 42, 44, 7, 16, 25, 34, 52, 61, 70, 79, 33, 35, 51, 53],
        [36, 37, 38, 39, 40, 41, 42, 43, 8, 17, 26, 35, 53, 62, 71, 80, 33, 34, 51, 52],

        [46, 47, 48, 49, 50, 51, 52, 53, 0, 9, 18, 27, 36, 54, 63, 72, 28, 29, 37, 38],
        [45, 47, 48, 49, 50, 51, 52, 53, 1, 10, 19, 28, 37, 55, 64, 73, 27, 29, 36, 38],
        [45, 46, 48, 49, 50, 51, 52, 53, 2, 11, 20, 29, 38, 56, 65, 74, 27, 28, 36, 37],
        [45, 46, 47, 49, 50, 51, 52, 53, 3, 12, 21, 30, 39, 57, 66, 75, 31, 32, 40, 41],
        [45, 46, 47, 48, 50, 51, 52, 53, 4, 13, 22, 31, 40, 58, 67, 76, 30, 32, 39, 41],
        [45, 46, 47, 48, 49, 51, 52, 53, 5, 14, 23, 32, 41, 59, 68, 77, 30, 31, 39, 40],
        [45, 46, 47, 48, 49, 50, 52, 53, 6, 15, 24, 33, 42, 60, 69, 78, 34, 35, 43, 44],
        [45, 46, 47, 48, 49, 50, 51, 53, 7, 16, 25, 34, 43, 61, 70, 79, 33, 35, 42, 44],
        [45, 46, 47, 48, 49, 50, 51, 52, 8, 17, 26, 35, 44, 62, 71, 80, 33, 34, 42, 43],

        [55, 56, 57, 58, 59, 60, 61, 62, 0, 9, 18, 27, 36, 45, 63, 72, 64, 65, 73, 74],
        [54, 56, 57, 58, 59, 60, 61, 62, 1, 10, 19, 28, 37, 46, 64, 73, 63, 65, 72, 74],
        [54, 55, 57, 58, 59, 60, 61, 62, 2, 11, 20, 29, 38, 47, 65, 74, 63, 64, 72, 73],
        [54, 55, 56, 58, 59, 60, 61, 62, 3, 12, 21, 30, 39, 48, 66, 75, 67, 68, 76, 77],
        [54, 55, 56, 57, 59, 60, 61, 62, 4, 13, 22, 31, 40, 49, 67, 76, 66, 68, 75, 77],
        [54, 55, 56, 57, 58, 60, 61, 62, 5, 14, 23, 32, 41, 50, 68, 77, 66, 67, 75, 76],
        [54, 55, 56, 57, 58, 59, 61, 62, 6, 15, 24, 33, 42, 51, 69, 78, 70, 71, 79, 80],
        [54, 55, 56, 57, 58, 59, 60, 62, 7, 16, 25, 34, 43, 52, 70, 79, 69, 71, 78, 80],
        [54, 55, 56, 57, 58, 59, 60, 61, 8, 17, 26, 35, 44, 53, 71, 80, 69, 70, 78, 79],

        [64, 65, 66, 67, 68, 69, 70, 71, 0, 9, 18, 27, 36, 45, 54, 72, 55, 56, 73, 74],
        [63, 65, 66, 67, 68, 69, 70, 71, 1, 10, 19, 28, 37, 46, 55, 73, 54, 56, 72, 74],
        [63, 64, 66, 67, 68, 69, 70, 71, 2, 11, 20, 29, 38, 47, 56, 74, 54, 55, 72, 73],
        [63, 64, 65, 67, 68, 69, 70, 71, 3, 12, 21, 30, 39, 48, 57, 75, 58, 59, 76, 77],
        [63, 64, 65, 66, 68, 69, 70, 71, 4, 13, 22, 31, 40, 49, 58, 76, 57, 59, 75, 77],
        [63, 64, 65, 66, 67, 69, 70, 71, 5, 14, 23, 32, 41, 50, 59, 77, 57, 58, 75, 76],
        [63, 64, 65, 66, 67, 68, 70, 71, 6, 15, 24, 33, 42, 51, 60, 78, 61, 62, 79, 80],
        [63, 64, 65, 66, 67, 68, 69, 71, 7, 16, 25, 34, 43, 52, 61, 79, 60, 62, 78, 80],
        [63, 64, 65, 66, 67, 68, 69, 70, 8, 17, 26, 35, 44, 53, 62, 80, 60, 61, 78, 79],

        [73, 74, 75, 76, 77, 78, 79, 80, 0, 9, 18, 27, 36, 45, 54, 63, 55, 56, 64, 65],
        [72, 74, 75, 76, 77, 78, 79, 80, 1, 10, 19, 28, 37, 46, 55, 64, 54, 56, 63, 65],
        [72, 73, 75, 76, 77, 78, 79, 80, 2, 11, 20, 29, 38, 47, 56, 65, 54, 55, 63, 64],
        [72, 73, 74, 76, 77, 78, 79, 80, 3, 12, 21, 30, 39, 48, 57, 66, 58, 59, 67, 68],
        [72, 73, 74, 75, 77, 78, 79, 80, 4, 13, 22, 31, 40, 49, 58, 67, 57, 59, 66, 68],
        [72, 73, 74, 75, 76, 78, 79, 80, 5, 14, 23, 32, 41, 50, 59, 68, 57, 58, 66, 67],
        [72, 73, 74, 75, 76, 77, 79, 80, 6, 15, 24, 33, 42, 51, 60, 69, 61, 62, 70, 71],
        [72, 73, 74, 75, 76, 77, 78, 80, 7, 16, 25, 34, 43, 52, 61, 70, 60, 62, 69, 71],
        [72, 73, 74, 75, 76, 77, 78, 79, 8, 17, 26, 35, 44, 53, 62, 71, 60, 61, 69, 70]
    ];
}


internal class SudokuPuzzles
{
    public string[] Col17ClueSudokus { get; set; } = new string[]
    {
        "000000010400000000020000000000050407008000300001090000300400200050100000000806000",
        "000000010400000000020000000000050604008000300001090000300400200050100000000807000",
        "000000012000035000000600070700000300000400800100000000000120000080000040050000600",
        "000000012003600000000007000410020000000500300700000600280000040000300500000000000",
        "000000012008030000000000040120500000000004700060000000507000300000620000000100000",
        "000000012040050000000009000070600400000100000000000050000087500601000300200000000",
        "000000012050400000000000030700600400001000000000080000920000800000510700000003000",
        "000000012300000060000040000900000500000001070020000000000350400001400800060000000",
        "000000012400090000000000050070200000600000400000108000018000000000030700502000000",
        "000000012500008000000700000600120000700000450000030000030000800000500700020000000",
        "000000012700060000000000050080200000600000400000109000019000000000030800502000000",
        "000000012800040000000000060090200000700000400000501000015000000000030900602000000",
        "000000012980000000000600000100700080402000000000300600070000300050040000000010000",
        "000000013000030080070000000000206000030000900000010000600500204000400700100000000",
        "000000013000200000000000080000760200008000400010000000200000750600340000000008000",
        "000000013000500070000802000000400900107000000000000200890000050040000600000010000",
        "000000013000700060000508000000400800106000000000000200740000050020000400000010000",
        "000000013000700060000509000000400900106000000000000200740000050080000400000010000",
        "000000013000800070000502000000400900107000000000000200890000050040000600000010000",
        "000000013020500000000000000103000070000802000004000000000340500670000200000010000",
    };
    public string[] GetSudokuPuzzles()
    {
        List<string> puzzles = new List<string>();
        foreach (var puzzle in Col17ClueSudokus)
        {
            for (int i = 0; i < 50; i++)
            {
                puzzles.Add(puzzle);
            }
        }
        return puzzles.ToArray();
    }
}