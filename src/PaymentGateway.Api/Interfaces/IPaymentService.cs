using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Models.Responses;

namespace PaymentGateway.Api.Interfaces;

public interface IPaymentService
{
    public Task<PostPaymentResponse?> GetPaymentDataAsync(Guid paymentId);
    public Task<PostPaymentResponse> SendPaymentRequestAsync(PostPaymentRequest request);
}