using System;
using System.Collections.Generic;
using Smartwyre.DeveloperTest.Calculators;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Factories;

public class RebateCalculatorFactory : IRebateCalculatorFactory
{
    private readonly Dictionary<IncentiveType, Lazy<IRebateCalculator>> rebateCalculators = new()
    {
        { IncentiveType.FixedCashAmount, new Lazy<IRebateCalculator>(() => new FixedCashAmountRebateCalculator())},
        { IncentiveType.FixedRateRebate, new Lazy<IRebateCalculator>(() => new FixedRateRebateServiceCalculator())},
        { IncentiveType.AmountPerUom, new Lazy<IRebateCalculator>(() => new AmountPerUomRebateCalculator())},
    };

    public RebateCalculatorFactory()
    {
        
    }

    public IRebateCalculator GetRebateCalculator(IncentiveType type)
        => rebateCalculators.TryGetValue(type, out var calculator) 
            ? calculator.Value 
            : null;
}