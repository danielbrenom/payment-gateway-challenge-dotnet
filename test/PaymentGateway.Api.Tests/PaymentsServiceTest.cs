using Flurl.Http.Testing;

using Microsoft.Extensions.Configuration;

using PaymentGateway.Api.Enums;
using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Services;

namespace PaymentGateway.Api.Tests;

public class PaymentsServiceTest
{
    private readonly Random _random = new();

    [Fact]
    public async Task CreatesAcceptedPaymentSuccessfully()
    {
        using var httpTest = new HttpTest();
        httpTest.RespondWith("{\"authorized\": true,\"authorization_code\":\"123\"}");
        var payment = new PostPaymentRequest
        {
            ExpiryYear = _random.Next(DateTime.Now.Year + 1, 2030),
            ExpiryMonth = _random.Next(1, 12),
            Amount = _random.Next(1, 10000),
            CardNumber = "12345678901234",
            Currency = "GBP",
            Cvv = 123
        };
        var inMemorySettings = new Dictionary<string, string?> { { "BankUrl", "http://localhost:5000" } };
        var configuration = new ConfigurationBuilder()
                           .AddInMemoryCollection(inMemorySettings)
                           .Build();

        var service = new PaymentService(configuration, new PaymentsRepository());


        var response = await service.SendPaymentRequestAsync(payment);
        Assert.NotNull(response);
        Assert.Equal(PaymentStatus.Authorized, response.Status);
    }
    
    [Fact]
    public async Task CreatesRejectedPaymentSuccessfully()
    {
        using var httpTest = new HttpTest();
        httpTest.RespondWith("{\"authorized\": false,\"authorization_code\":\"123\"}");
        var payment = new PostPaymentRequest
        {
            ExpiryYear = _random.Next(DateTime.Now.Year + 1, 2030),
            ExpiryMonth = _random.Next(1, 12),
            Amount = _random.Next(1, 10000),
            CardNumber = "12345678901234",
            Currency = "GBP",
            Cvv = 123
        };
        var inMemorySettings = new Dictionary<string, string?> { { "BankUrl", "http://localhost:5000" } };
        var configuration = new ConfigurationBuilder()
                           .AddInMemoryCollection(inMemorySettings)
                           .Build();

        var service = new PaymentService(configuration, new PaymentsRepository());


        var response = await service.SendPaymentRequestAsync(payment);
        Assert.NotNull(response);
        Assert.Equal(PaymentStatus.Declined, response.Status);
    }
}