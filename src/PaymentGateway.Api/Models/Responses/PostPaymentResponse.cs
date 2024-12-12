using PaymentGateway.Api.Enums;
using PaymentGateway.Api.Models.Requests;

namespace PaymentGateway.Api.Models.Responses;

public class PostPaymentResponse
{
    public Guid Id { get; set; }

    public PaymentStatus Status { get; set; }

    public int CardNumberLastFour { get; set; }
    public int ExpiryMonth { get; set; }
    public int ExpiryYear { get; set; }
    public required string Currency { get; set; }
    public int Amount { get; set; }

    public static explicit operator PostPaymentResponse(PostPaymentRequest request)
        => new()
        {
            Id = Guid.NewGuid(),
            CardNumberLastFour = int.Parse(request.CardNumber[^4..]),
            ExpiryMonth = request.ExpiryMonth,
            ExpiryYear = request.ExpiryYear,
            Currency = request.Currency,
            Amount = request.Amount
        };
}