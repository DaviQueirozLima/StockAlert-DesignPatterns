namespace StockAlert.Application.Strategies;

public static class NotificationMessageBuilder
{
    public static string Build(Domain.Entities.AlertRule rule, decimal currentPrice)
    {
        var condition = rule.TargetPrice.HasValue
            ? $"Preço alvo: R$ {rule.TargetPrice.Value:F2}"
            : $"Variação alvo: {rule.PercentageChange:F2}%";

        return $"""
                Olá, {rule.User!.Name}!

                A ação {rule.StockSymbol} atingiu a condição configurada.

                Preço atual: R$ {currentPrice:F2}
                {condition}
                Operador: {rule.Operator}

                Data: {DateTime.Now:dd/MM/yyyy HH:mm}

                StockAlert
                """;
    }
}