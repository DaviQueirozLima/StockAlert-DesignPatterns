using StockAlert.Domain.Enums;
using StockAlert.Domain.Strategies;

namespace StockAlert.Application.Strategies;

public class NotificationStrategyFactory : INotificationStrategyFactory
{
    private readonly IEnumerable<INotificationStrategy> _strategies;

    public NotificationStrategyFactory(IEnumerable<INotificationStrategy> strategies)
    {
        _strategies = strategies;
    }

    public INotificationStrategy GetStrategy(NotificationChannel channel)
    {
        var strategy = _strategies.FirstOrDefault(s => s.Channel == channel);

        if (strategy is null)
            throw new InvalidOperationException(
                $"Nenhuma estratégia encontrada para o canal {channel}."
            );

        return strategy;
    }
}