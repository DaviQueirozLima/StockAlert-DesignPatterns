using StockAlert.Domain.Enums;
using StockAlert.Domain.Events;

namespace StockAlert.Domain.Strategies;

public interface INotificationStrategy
{
    NotificationChannel Channel { get; }

    Task SendAsync(AlertTriggeredEvent alertEvent);
}