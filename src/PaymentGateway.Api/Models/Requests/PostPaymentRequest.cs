using System.Text.Json.Serialization;

namespace PaymentGateway.Api.Models.Requests;

public class PostPaymentRequest
{
    [JsonPropertyName("cardNumber")]
    public required string CardNumber { get; set; }

    [JsonPropertyName("expiryMonth")]
    public int ExpiryMonth { get; set; }

    [JsonPropertyName("expiryYear")]
    public int ExpiryYear { get; set; }

    [JsonPropertyName("currency")]
    public required string Currency { get; set; }

    public int Amount { get; set; }
    public int Cvv { get; set; }
}