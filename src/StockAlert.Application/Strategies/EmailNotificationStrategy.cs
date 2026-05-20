using StockAlert.Domain.Enums;
using StockAlert.Domain.Events;
using StockAlert.Domain.Services;
using StockAlert.Domain.Strategies;

namespace StockAlert.Application.Strategies;

public class EmailNotificationStrategy(IEmailService emailService) : INotificationStrategy
{
    public NotificationChannel Channel => NotificationChannel.Email;

    public async Task SendAsync(AlertTriggeredEvent alertEvent)
    {
        var rule = alertEvent.AlertRule;
        var quote = alertEvent.Quote;

        var subject = $"Alerta de ação: {rule.StockSymbol}";

        var message = NotificationMessageBuilder.Build(rule, quote.Price);

        await emailService.SendAlertEmailAsync(
            rule.User!.Email,
            subject,
            message
        );
    }
}