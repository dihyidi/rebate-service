using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Calculators;

public class FixedRateRebateServiceCalculator : IRebateCalculator
{
    public RebateCalculationResult Calculate(CalculateRebateRequest request, Rebate rebate, Product product)
    {
        RebateCalculationResult result = new();

        var hasFlag = product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedRateRebate);
        var isEmpty = rebate.Percentage == 0 || product.Price == 0 || request.Volume == 0;

        if (!hasFlag || isEmpty)
        {
            result.Success = false;
        }
        else
        {
            result.RebateAmount += product.Price * rebate.Percentage * request.Volume;
            result.Success = true;
        }

        return result;
    }
}