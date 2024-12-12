using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

using Moq;

using PaymentGateway.Api.Controllers;
using PaymentGateway.Api.Interfaces;
using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Models.Responses;
using PaymentGateway.Api.Services;

namespace PaymentGateway.Api.Tests;

public class PaymentsControllerTests
{
    private readonly Random _random = new();

    [Fact]
    public async Task CreatesPaymentSuccessfully()
    {
        var serviceMock = new Mock<IPaymentService>();
        var payment = new PostPaymentRequest
        {
            ExpiryYear = _random.Next(DateTime.Now.Year + 1, 2030),
            ExpiryMonth = _random.Next(1, 12),
            Amount = _random.Next(1, 10000),
            CardNumber = "12345678901234",
            Currency = "GBP",
            Cvv = 123
        };
        serviceMock.Setup(m => m.SendPaymentRequestAsync(It.IsAny<PostPaymentRequest>()))
                   .ReturnsAsync((PostPaymentResponse)payment);

        var paymentsRepository = new PaymentsRepository();
        var webApplicationFactory = new WebApplicationFactory<PaymentsController>();
        var client = webApplicationFactory.WithWebHostBuilder(builder =>
                                           {
                                               builder.ConfigureServices(services =>
                                               {
                                                   ((ServiceCollection)services).AddSingleton(paymentsRepository);
                                                   ((ServiceCollection)services).AddSingleton(serviceMock.Object);
                                               });
                                           })
                                          .CreateClient();

        var response = await client.PostAsync($"/api/Payments", new StringContent(JsonSerializer.Serialize(payment), Encoding.UTF8, "application/json"));
        var paymentResponse = await response.Content.ReadFromJsonAsync<PostPaymentResponse>();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(paymentResponse);
    }

    [Fact]
    public async Task RetrievesAPaymentSuccessfully()
    {
        // Arrange
        var payment = new PostPaymentResponse
        {
            Id = Guid.NewGuid(),
            ExpiryYear = _random.Next(DateTime.Now.Year, 2030),
            ExpiryMonth = _random.Next(1, 12),
            Amount = _random.Next(1, 10000),
            CardNumberLastFour = _random.Next(1111, 9999),
            Currency = "GBP"
        };

        var paymentsRepository = new PaymentsRepository();
        paymentsRepository.Add(payment);

        var webApplicationFactory = new WebApplicationFactory<PaymentsController>();
        var client = webApplicationFactory.WithWebHostBuilder(builder => builder.ConfigureServices(services => ((ServiceCollection)services).AddSingleton(paymentsRepository)))
                                          .CreateClient();

        // Act
        var response = await client.GetAsync($"/api/Payments/{payment.Id}");
        var paymentResponse = await response.Content.ReadFromJsonAsync<PostPaymentResponse>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(paymentResponse);
    }

    [Fact]
    public async Task Returns404IfPaymentNotFound()
    {
        // Arrange
        var webApplicationFactory = new WebApplicationFactory<PaymentsController>();
        var client = webApplicationFactory.CreateClient();

        // Act
        var response = await client.GetAsync($"/api/Payments/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}