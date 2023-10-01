using System;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Factories;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services;

public class RebateService : IRebateService
{
    private readonly IRebateDataStore rebateDataStore;
    private readonly IProductDataStore productDataStore;
    private readonly IRebateCalculatorFactory rebateCalculatorFactory;

    public RebateService(IRebateDataStore rebateDataStore, 
        IProductDataStore productDataStore, 
        IRebateCalculatorFactory rebateCalculatorFactory)
    {
        this.rebateDataStore = rebateDataStore;
        this.productDataStore = productDataStore;
        this.rebateCalculatorFactory = rebateCalculatorFactory;
    }

    public CalculateRebateResult Calculate(CalculateRebateRequest request)
    {
        var result = new CalculateRebateResult();
        
        Rebate rebate = rebateDataStore.GetRebate(request.RebateIdentifier);
        Product product = productDataStore.GetProduct(request.ProductIdentifier);

        if (rebate is null || product is null)
        {
            result.Success = false;
            return result;
        }

        var rebateCalculator = rebateCalculatorFactory.GetRebateCalculator(rebate.Incentive);
        if (rebateCalculator is null)
        {
            result.Success = false;
            return result;
        }
        var calculationResult = rebateCalculator.Calculate(request, rebate, product);

        if (calculationResult.Success)
        {
            rebateDataStore.StoreCalculationResult(rebate, calculationResult.RebateAmount);
        }

        return MapToCalculateRebateResult(calculationResult);
    }

    private static CalculateRebateResult MapToCalculateRebateResult(RebateCalculationResult calculationResult) =>
        new()
        {
            Success = calculationResult.Success
        };
}

