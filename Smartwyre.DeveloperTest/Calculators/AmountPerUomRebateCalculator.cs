using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Calculators;

public class AmountPerUomRebateCalculator : IRebateCalculator
{
    public RebateCalculationResult Calculate(CalculateRebateRequest request, Rebate rebate, Product product)
    {
        RebateCalculationResult result = new();

        var hasFlag = product.SupportedIncentives.HasFlag(SupportedIncentiveType.AmountPerUom);
        var isEmpty = rebate.Amount == 0 || request.Volume == 0;

        if (!hasFlag || isEmpty)
        {
            result.Success = false;
        }
        else
        {
            result.RebateAmount = rebate.Amount * request.Volume;
            result.Success = true;
        }

        return result;
    }
}