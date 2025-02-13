using DreamOn.HelloSudoku;
namespace DreamOn.HelloSudokuWorker;
public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                var intervalResults = new SudokuEngine_20250130(new()).StartProcessingSudokuPuzzles();
                _logger.LogInformation("Result: {result}", intervalResults.ToJson());
            }
            await Task.Delay(5000, stoppingToken);
        }
    }
}
