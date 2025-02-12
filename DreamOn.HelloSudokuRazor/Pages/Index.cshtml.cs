using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DreamOn.HelloSudoku;
namespace DreamOn.HelloSudokuRazor.Pages;
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    public string HelloSudokuResult { get; set; }
    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }
    public void OnGet()
    {
        var result = (new SudokuEngine_20250130(new())).StartProcessingSudokuPuzzles();
        HelloSudokuResult = result.ToJson();
    }
}
