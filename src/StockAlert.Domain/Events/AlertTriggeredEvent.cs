
using StockAlert.Domain.Entities;
using StockAlert.Domain.Services.Dtos;

namespace StockAlert.Domain.Events;

public class AlertTriggeredEvent(AlertRule alertRule, StockQuoteDto quote)
{
    public AlertRule AlertRule { get; } = alertRule;
    public StockQuoteDto Quote { get; } = quote;
    public DateTime TriggeredAt { get; } = DateTime.UtcNow;
}