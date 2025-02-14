if ($false) 
{
    Add-Type -Path C:\Users\Silvero\GitHub\DreamOn.HelloSudoku\DreamOn.HelloSudokuLib\bin\Debug\net9.0\DreamOn.HelloSudokuLib.dll
    $runProperties = New-Object DreamOn.HelloSudokuLib.RunProperties
    $SudokuEngine = New-Object DreamOn.HelloSudokuLib.SudokuEngine_20250130($runProperties)
    $SudokuEngine.StartProcessingSudokuPuzzles()
}

if ($true) 
{
    Add-Type -Path C:\Users\Silvero\GitHub\DreamOn.HelloSudoku\DreamOn.HelloSudokuLib\bin\Debug\net9.0\DreamOn.HelloSudokuLib.dll
    $runProperties = New-Object DreamOn.HelloSudokuLib.RunProperties
    $SudokuEngine = New-Object DreamOn.HelloSudokuLib.SudokuEngine_20250130($runProperties)
    $sudokuResult = $SudokuEngine.StartProcessingSudokuPuzzles()
    $sudokuResult
    $sudokuResult | ConvertTo-Csv
    $sudokuResult | ConvertTo-Html
    $sudokuResult | ConvertTo-Json
}

if ($false) 
{
    [DreamOn.HelloSudokuLib.RunProperties] | Get-Member
    [DreamOn.HelloSudokuLib.RunProperties] | Get-Member -Static
}