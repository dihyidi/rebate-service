using Smartwyre.DeveloperTest.Calculators;
using Smartwyre.DeveloperTest.Tests.Calculators.Common;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Calculators;

public class FixedCashAmountRebateCalculatorTests
{
    private readonly FixedCashAmountRebateCalculator rebateCalculator;
    
    public FixedCashAmountRebateCalculatorTests()
    {
        rebateCalculator = new FixedCashAmountRebateCalculator();
    }

    [Fact]
    public void Calculate_NotSupportedFlag_ResultUnsuccessful()
    {
        // arrange
        var product = Setup.GetProduct(SupportedIncentiveType.FixedRateRebate);
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
        var product = Setup.GetProduct(SupportedIncentiveType.FixedCashAmount);
        var request = Setup.GetCalculateRebateRequest(9);
        var rebate = Setup.GetRebate(0);

        // act
        var result = rebateCalculator.Calculate(request, rebate, product);

        // assert
        Assert.False(result.Success);
    }

    [Fact]
    public void Calculate_ValidParams_ResultSuccessful()
    {
        // arrange
        var product = Setup.GetProduct(SupportedIncentiveType.FixedCashAmount);
        var request = Setup.GetCalculateRebateRequest(1);
        var rebate = Setup.GetRebate(9);

        var expectedRebateAmount = 9;

        // act
        var result = rebateCalculator.Calculate(request, rebate, product);

        // assert
        Assert.True(result.Success);
        Assert.Equal(expectedRebateAmount, result.RebateAmount);
    }


}