using StockAlert.Domain.Enums;
using StockAlert.Domain.Events;
using StockAlert.Domain.Strategies;

namespace StockAlert.Application.Strategies;

public class UnsupportedWhatsappNotificationStrategy : INotificationStrategy
{
    public NotificationChannel Channel => NotificationChannel.Whatsapp;

    public Task SendAsync(AlertTriggeredEvent alertEvent)
    {
        throw new NotSupportedException(
            "Notificação por WhatsApp não está disponível neste protótipo."
        );
    }
}