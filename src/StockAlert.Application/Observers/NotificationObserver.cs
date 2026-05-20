using Microsoft.Extensions.Logging;
using StockAlert.Domain.Enums;
using StockAlert.Domain.Events;
using StockAlert.Domain.Observers;
using StockAlert.Domain.Strategies;

namespace StockAlert.Application.Observers;

public class NotificationObserver : IAlertObserver
{
    private readonly INotificationStrategyFactory _strategyFactory;
    private readonly ILogger<NotificationObserver> _logger;

    public NotificationObserver(INotificationStrategyFactory strategyFactory, ILogger<NotificationObserver> logger)
    {
        _strategyFactory = strategyFactory;
        _logger = logger;
    }

    public async Task UpdateAsync(AlertTriggeredEvent alertEvent)
    {
        var channel = alertEvent.AlertRule.PreferredChannel ?? NotificationChannel.Email;

        var strategy = _strategyFactory.GetStrategy(channel);

        await strategy.SendAsync(alertEvent);
    }
}