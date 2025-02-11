using System.Diagnostics;
using DreamOn.HelloSudokuConsole.Engine;
Console.WriteLine("Start ############################################################################################################################################");
RunProperties runProperties = new();
Console.WriteLine($"{runProperties.ToJson()} {nameof(SudokuEngine_20250130)} debugger:{Debugger.IsAttached}");
var intervalResults = new SudokuEngine_20250130(runProperties).StartProcessingSudokuPuzzles();
Console.WriteLine("Finished #########################################################################################################################################");
Console.WriteLine(intervalResults.ToJson());