using StockAlert.Domain.Enums;

namespace StockAlert.Domain.Strategies;

public interface INotificationStrategyFactory
{
    INotificationStrategy GetStrategy(NotificationChannel channel);
}