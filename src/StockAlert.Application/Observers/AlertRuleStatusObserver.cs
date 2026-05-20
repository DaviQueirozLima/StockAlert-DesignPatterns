using StockAlert.Domain.Events;
using StockAlert.Domain.Observers;
using StockAlert.Domain.Repositories;

namespace StockAlert.Application.Observers;

public class AlertRuleStatusObserver : IAlertObserver
{
    private readonly IAlertRuleRepository _alertRuleRepository;

    public AlertRuleStatusObserver(IAlertRuleRepository alertRuleRepository)
    {
        _alertRuleRepository = alertRuleRepository;
    }

    public async Task UpdateAsync(AlertTriggeredEvent alertEvent)
    {
        var rule = alertEvent.AlertRule;

        rule.LastTriggeredAt = DateTime.UtcNow;

        if (rule.NotifyOnce)
            rule.IsActive = false;

        await _alertRuleRepository.UpdateAsync(rule);
    }
}