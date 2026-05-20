using Microsoft.Extensions.Options;
using StockAlert.API.Configurations;
using StockAlert.Application.Observers;
using StockAlert.Domain.Entities;
using StockAlert.Domain.Events;
using StockAlert.Domain.Repositories;
using StockAlert.Domain.Services;

namespace StockAlert.API.Workers;

public class StockMonitorWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<StockMonitorWorker> _logger;
    private readonly WorkerSettings _settings;

    public StockMonitorWorker(
        IServiceProvider serviceProvider,
        ILogger<StockMonitorWorker> logger,
        IOptions<WorkerSettings> options)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _settings = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Stock Monitor Worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();

                var alertRuleRepository = scope.ServiceProvider.GetRequiredService<IAlertRuleRepository>();
                var brapiService = scope.ServiceProvider.GetRequiredService<IBrapiService>();
                var conditionChecker = scope.ServiceProvider.GetRequiredService<IAlertConditionChecker>();
                var publisher = scope.ServiceProvider.GetRequiredService<AlertEventPublisher>();

                var activeRules = await alertRuleRepository.GetAllActiveAsync();

                foreach (var rule in activeRules)
                {
                    var quote = await brapiService.GetStockQuoteAsync(rule.StockSymbol);

                    if (quote is null)
                        continue;

                    var conditionMet = conditionChecker.IsConditionMet(
                        quote.Price,
                        quote.PreviousClose,
                        rule
                    );

                    if (!conditionMet)
                        continue;

                    if (!CanSendNotification(rule))
                        continue;

                    var alertEvent = new AlertTriggeredEvent(rule, quote);

                    await publisher.NotifyAsync(alertEvent);
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error while monitoring stock alerts.");
            }

            await Task.Delay(
                TimeSpan.FromSeconds(_settings.IntervalSeconds),
                stoppingToken
            );
        }
    }

    private static bool CanSendNotification(AlertRule rule)
    {
        if (rule.LastTriggeredAt is null)
            return true;

        var cooldownMinutes = rule.CooldownMinutes ?? 15;

        return DateTime.UtcNow >= rule.LastTriggeredAt.Value.AddMinutes(cooldownMinutes);
    }
}