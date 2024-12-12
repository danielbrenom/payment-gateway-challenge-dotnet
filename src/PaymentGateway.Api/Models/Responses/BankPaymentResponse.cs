using System.Text.Json.Serialization;

namespace PaymentGateway.Api.Models.Responses;

public class BankPaymentResponse
{
    public bool Authorized { get; set; }

    [JsonPropertyName("authorization_code")]
    public required string AuthorizationCode { get; set; }
}