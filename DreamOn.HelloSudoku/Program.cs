using System.Diagnostics;
using DreamOn.HelloSudoku;
Console.WriteLine("Start ############################################################################################################################################");
Console.WriteLine($"{runProperties.ToJson()} {nameof(SudokuEngine_20250130)} debugger:{Debugger.IsAttached}");
var intervalResults = new SudokuEngine_20250130(new()).StartProcessingSudokuPuzzles();
Console.WriteLine("Finished #########################################################################################################################################");
Console.WriteLine(intervalResults.ToJson());
