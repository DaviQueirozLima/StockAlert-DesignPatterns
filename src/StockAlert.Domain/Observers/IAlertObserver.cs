using StockAlert.Domain.Events;

namespace StockAlert.Domain.Observers;

public interface IAlertObserver
{
    Task UpdateAsync(AlertTriggeredEvent alertEvent);
}