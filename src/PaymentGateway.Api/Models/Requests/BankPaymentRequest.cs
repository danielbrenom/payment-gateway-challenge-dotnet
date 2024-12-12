using System.Text.Json.Serialization;

namespace PaymentGateway.Api.Models.Requests;

public class BankPaymentRequest
{
    [JsonPropertyName("card_number")]
    public required string CardNumber { get; set; }

    [JsonPropertyName("expiry_date")]
    public required string ExpiryDate { get; set; }

    public required string Currency { get; set; }
    public required int Amount { get; set; }
    public required string Cvv { get; set; }

    public static explicit operator BankPaymentRequest(PostPaymentRequest request)
        => new()
        {
            CardNumber = request.CardNumber.ToString(),
            ExpiryDate = $"{request.ExpiryMonth:D2}/{request.ExpiryYear:D4}",
            Currency = request.Currency.ToUpper(),
            Amount = request.Amount,
            Cvv = request.Cvv.ToString()
        };
}