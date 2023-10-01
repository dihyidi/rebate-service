using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Smartwyre.DeveloperTest.Calculators;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Factories;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddScoped<IRebateService, RebateService>();
builder.Services.AddScoped<IRebateDataStore, RebateDataStore>();
builder.Services.AddScoped<IProductDataStore, ProductDataStore>();

builder.Services.AddSingleton<IRebateCalculatorFactory, RebateCalculatorFactory>();

builder.Services.AddScoped<IRebateCalculator, AmountPerUomRebateCalculator>();
builder.Services.AddScoped<IRebateCalculator, FixedCashAmountRebateCalculator>();
builder.Services.AddScoped<IRebateCalculator, FixedRateRebateServiceCalculator>();

using var host = builder.Build();

Run(host.Services);

await host.RunAsync();

static void Run(IServiceProvider hostProvider)
{
    using var serviceScope = hostProvider.CreateScope();
    var provider = serviceScope.ServiceProvider;
    var rebateService = provider.GetRequiredService<IRebateService>();

    try
    {
        Console.WriteLine("\nPlease enter rebate identifier:");
        var rebateIdentifier = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(rebateIdentifier)) throw new ArgumentException("Please enter valid value.");

        Console.WriteLine("\nPlease enter product identifier:");
        var productIdentifier = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(productIdentifier)) throw new ArgumentException("Please enter valid value.");

        Console.WriteLine("\nPlease enter volume:");
        var volume = Convert.ToDecimal(Console.ReadLine());
        if (volume <= 0) throw new ArgumentException("Please enter valid value.");

        var rebate = rebateService.Calculate(new CalculateRebateRequest()
        {
            ProductIdentifier = productIdentifier,
            RebateIdentifier = rebateIdentifier,
            Volume = volume
        });

        Console.WriteLine(rebate.Success);
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
}
