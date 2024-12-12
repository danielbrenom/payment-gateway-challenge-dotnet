using Flurl;
using Flurl.Http;

using PaymentGateway.Api.Enums;
using PaymentGateway.Api.Interfaces;
using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Models.Responses;

namespace PaymentGateway.Api.Services;

public class PaymentService(IConfiguration configuration, PaymentsRepository paymentsRepository) : IPaymentService
{
    private readonly string _baseUrl = configuration.GetValue<string>("BankUrl") ?? throw new ArgumentException("Bank url is missing");
    public Task<PostPaymentResponse?> GetPaymentDataAsync(Guid paymentId)
        => Task.FromResult(paymentsRepository.Get(paymentId));

    public async Task<PostPaymentResponse> SendPaymentRequestAsync(PostPaymentRequest request)
    {
        var bankRequest = (BankPaymentRequest)request;
        var responseRequest = await _baseUrl.AppendPathSegment("payments").PostJsonAsync(bankRequest);
        var responseData = await responseRequest.GetJsonAsync<BankPaymentResponse>();
        var paymentEntity = (PostPaymentResponse)request;
        paymentEntity.Status = responseData!.Authorized ? PaymentStatus.Authorized : PaymentStatus.Declined;

        paymentsRepository.Add(paymentEntity);
        return paymentEntity;
    }
}