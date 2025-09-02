using InnovaGraphics.Repositories.Interfaces.InnovaGraphics.Repositories.Interfaces;

public class ExpiredTokenCleanupService : BackgroundService
{
    private readonly ILogger<ExpiredTokenCleanupService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory; 

    private readonly TimeSpan _cleanupInterval = TimeSpan.FromMinutes(30); 

    public ExpiredTokenCleanupService(
        ILogger<ExpiredTokenCleanupService> logger,
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Сервіс очищення прострочених токенів запущено.");

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Запуск очищення прострочених токенів...");

            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var tokenManagerRepository = scope.ServiceProvider.GetRequiredService<ITokenManagerRepository>();

                    var cutoffTime = DateTimeOffset.UtcNow;

                    await tokenManagerRepository.DeleteExpiredTokensAsync(cutoffTime);

                    _logger.LogInformation("Очищення прострочених токенів завершено. Видалено токени до {CutoffTime}.", cutoffTime);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Помилка під час виконання завдання очищення прострочених токенів.");
            }

            await Task.Delay(_cleanupInterval, stoppingToken);
        }

        _logger.LogInformation("Сервіс очищення прострочених токенів зупинено.");
    }
}