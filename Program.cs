using System.Diagnostics;
using DreamOn.HelloSudoku.Engine;
Console.WriteLine("Start ############################################################################################################################################");
RunProperties runProperties = new();
Console.WriteLine($"{runProperties.ToJson()} {nameof(SudokuEngine_20250130)} debugger:{Debugger.IsAttached}");
new SudokuEngine_20250130(runProperties).StartProcessingFile();
Console.WriteLine("Finished #########################################################################################################################################");
