using Smartwyre.DeveloperTest.Calculators;
using Smartwyre.DeveloperTest.Tests.Calculators.Common;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Calculators;

public class FixedRateRebateServiceCalculatorTests
{
    private readonly FixedRateRebateServiceCalculator rebateCalculator;
    
    public FixedRateRebateServiceCalculatorTests()
    {
        rebateCalculator = new FixedRateRebateServiceCalculator();
    }

    [Fact]
    public void Calculate_NotSupportedFlag_ResultUnsuccessful()
    {
        // arrange
        var product = Setup.GetProduct(SupportedIncentiveType.FixedCashAmount);
        var request = Setup.GetCalculateRebateRequest(1);
        var rebate = Setup.GetRebate(1);

        // act
        var result = rebateCalculator.Calculate(request, rebate, product);

        // assert
        Assert.False(result.Success);
    }

    [Fact]
    public void Calculate_Empty_ResultUnsuccessful()
    {
        // arrange
        var product = Setup.GetProduct(SupportedIncentiveType.FixedRateRebate, 0m);
        var request = Setup.GetCalculateRebateRequest(0);
        var rebate = Setup.GetRebate(1, 0m);

        // act
        var result = rebateCalculator.Calculate(request, rebate, product);

        // assert
        Assert.False(result.Success);
    }

    [Fact]
    public void Calculate_ValidParams_ResultSuccessful()
    {
        // arrange
        var product = Setup.GetProduct(SupportedIncentiveType.FixedRateRebate, 2m);
        var request = Setup.GetCalculateRebateRequest(3);
        var rebate = Setup.GetRebate(1, 6m);

        var expectedRebateAmount = 36;

        // act
        var result = rebateCalculator.Calculate(request, rebate, product);

        // assert
        Assert.True(result.Success);
        Assert.Equal(expectedRebateAmount, result.RebateAmount);
    }


}