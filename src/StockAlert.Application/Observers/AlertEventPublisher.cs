using StockAlert.Domain.Events;
using StockAlert.Domain.Observers;

namespace StockAlert.Application.Observers;

public class AlertEventPublisher(IEnumerable<IAlertObserver> observers)
{
    public async Task NotifyAsync(AlertTriggeredEvent alertEvent)
    {
        foreach (var observer in observers)
        {
            await observer.UpdateAsync(alertEvent);
        }
    }
}