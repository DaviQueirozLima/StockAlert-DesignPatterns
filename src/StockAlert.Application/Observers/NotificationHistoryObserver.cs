using StockAlert.Application.Strategies;
using StockAlert.Domain.Enums;
using StockAlert.Domain.Events;
using StockAlert.Domain.Observers;
using StockAlert.Domain.Repositories;

namespace StockAlert.Application.Observers;

public class NotificationHistoryObserver : IAlertObserver
{
    private readonly INotificationHistoryRepository _historyRepository;

    public NotificationHistoryObserver(INotificationHistoryRepository historyRepository)
    {
        _historyRepository = historyRepository;
    }

    public async Task UpdateAsync(AlertTriggeredEvent alertEvent)
    {
        var rule = alertEvent.AlertRule;
        var quote = alertEvent.Quote;

        var channel = rule.PreferredChannel ?? NotificationChannel.Email;

        var history = new Domain.Entities.NotificationHistory
        {
            AlertRuleId = rule.Id,
            UserId = rule.UserId,
            Channel = channel,
            Recipient = rule.User!.Email,
            Message = NotificationMessageBuilder.Build(rule, quote.Price),
            Success = true,
            Status = "Sent",
            SentAt = DateTime.UtcNow
        };

        await _historyRepository.AddAsync(history);
    }
}