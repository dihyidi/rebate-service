using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Calculators;

public class FixedCashAmountRebateCalculator : IRebateCalculator
{
    public RebateCalculationResult Calculate(CalculateRebateRequest request, Rebate rebate, Product product)
    {
        RebateCalculationResult result = new();

        var hasFlag = product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedCashAmount);
        
        if (!hasFlag || rebate.Amount == 0)
        {
            result.Success = false;
        }
        else
        {
            result.RebateAmount = rebate.Amount;
            result.Success = true;
        }

        return result;
    }
}