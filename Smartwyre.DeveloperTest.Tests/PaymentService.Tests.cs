using Moq;
using Smartwyre.DeveloperTest.Calculators;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Factories;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests;

public class RebateServiceTests
{
    private readonly Mock<IProductDataStore> productDataStoreMock;
    private readonly Mock<IRebateDataStore> rebateDataStoreMock;
    private readonly Mock<IRebateCalculatorFactory> rebateCalculatorFactoryMock;
    private readonly IRebateService rebateService;

    public RebateServiceTests()
    {
        productDataStoreMock = new Mock<IProductDataStore>(MockBehavior.Strict);
        rebateDataStoreMock = new Mock<IRebateDataStore>(MockBehavior.Strict);
        rebateCalculatorFactoryMock = new Mock<IRebateCalculatorFactory>(MockBehavior.Strict);

        rebateService = new RebateService(rebateDataStoreMock.Object, productDataStoreMock.Object, rebateCalculatorFactoryMock.Object);
    }
    
    [Fact]
    public void Calculate_RebateIsNull_ReturnsUnsuccessfulResult()
    {
        // arrange
        var request = GetCalculateRebateRequest();
        rebateDataStoreMock.Setup(x => x.GetRebate(It.IsAny<string>())).Returns((Rebate)null);
        productDataStoreMock.Setup(x => x.GetProduct(It.IsAny<string>())).Returns(new Product());
        
        // act
        var result = rebateService.Calculate(request);

        // assert
        Assert.False(result.Success);
    }
    
    [Fact]
    public void Calculate_ProductIsNull_ReturnsUnsuccessfulResult()
    {
        // arrange
        var request = GetCalculateRebateRequest();
        rebateDataStoreMock.Setup(x => x.GetRebate(It.IsAny<string>())).Returns(new Rebate());
        productDataStoreMock.Setup(x => x.GetProduct(It.IsAny<string>())).Returns((Product)null);
        
        // act
        var result = rebateService.Calculate(request);

        // assert
        Assert.False(result.Success);
    }
    
    [Fact]
    public void Calculate_CalculatorIsNull_ReturnsUnsuccessfulResult()
    {
        // arrange
        var request = GetCalculateRebateRequest();
        rebateDataStoreMock.Setup(x => x.GetRebate(It.IsAny<string>())).Returns(new Rebate
        {
            Incentive = IncentiveType.FixedCashAmount
        });
        productDataStoreMock.Setup(x => x.GetProduct(It.IsAny<string>())).Returns(new Product());
        rebateCalculatorFactoryMock.Setup(x => x.GetRebateCalculator(It.IsAny<IncentiveType>()))
            .Returns((IRebateCalculator)null);
        
        // act
        var result = rebateService.Calculate(request);

        // assert
        Assert.False(result.Success);
    }
    
    [Fact]
    public void Calculate_CalculationResultSuccess_ShouldCallStoreCalculationResult_ReturnsSuccessResult()
    {
        // arrange
        var request = GetCalculateRebateRequest();
        var rebate = new Rebate
        {
            Incentive = IncentiveType.FixedCashAmount
        };
        
        (Rebate rebate, decimal amount) expectedParams = (rebate, 10m);
        (Rebate rebate, decimal amount) actualParams = new();
        
        rebateDataStoreMock.Setup(x => x.GetRebate(It.IsAny<string>())).Returns(rebate);
        productDataStoreMock.Setup(x => x.GetProduct(It.IsAny<string>())).Returns(new Product());
        
        Mock<IRebateCalculator> rebateCalculator = new Mock<IRebateCalculator>(MockBehavior.Strict);
        rebateCalculator.Setup(x =>
                x.Calculate(It.IsAny<CalculateRebateRequest>(), It.IsAny<Rebate>(), It.IsAny<Product>()))
            .Returns(new RebateCalculationResult
            {
                Success = true,
                RebateAmount = expectedParams.amount
            });
        rebateCalculatorFactoryMock.Setup(x => x.GetRebateCalculator(It.IsAny<IncentiveType>()))
            .Returns(rebateCalculator.Object);

        rebateDataStoreMock.Setup(x => x.StoreCalculationResult(It.IsAny<Rebate>(), It.IsAny<decimal>()))
            .Callback<Rebate, decimal>((r, d) =>
            {
                actualParams.rebate = r;
                actualParams.amount = d;
            });
        
        // act
        var result = rebateService.Calculate(request);

        // assert
        Assert.Equal(expectedParams, actualParams);
        Assert.True(result.Success);
    }
    
    private static CalculateRebateRequest GetCalculateRebateRequest() => new()
    {
        ProductIdentifier = "product",
        RebateIdentifier = "rebate",
        Volume = 1m
    };
}
