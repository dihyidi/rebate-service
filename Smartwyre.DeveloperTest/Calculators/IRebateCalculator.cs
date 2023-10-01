using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Calculators;

public interface IRebateCalculator
{
    RebateCalculationResult Calculate(CalculateRebateRequest request, Rebate rebate, Product product);
}