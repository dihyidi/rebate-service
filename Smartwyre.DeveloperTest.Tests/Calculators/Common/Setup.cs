using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Tests.Calculators.Common;

public static class Setup
{
    public static Product GetProduct(SupportedIncentiveType type, decimal price = 1m) => new() { SupportedIncentives = type, Price = price};

    public static CalculateRebateRequest GetCalculateRebateRequest(int volume) => new() { Volume = volume };

    public static Rebate GetRebate(int amount, decimal percentage = 1m) => new() { Amount = amount, Percentage = percentage};
}