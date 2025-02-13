if ($false) 
{
    Add-Type -Path foo.dll
    $basicTestObject = New-Object BasicTest 
    $basicTestObject.Multiply(5, 2)

}