using System.ComponentModel;
using System.Text.Json.Serialization;

namespace PaymentGateway.Api.Enums;

[JsonConverter(typeof(JsonStringEnumConverter<PaymentStatus>))]
public enum PaymentStatus
{
    [Description("Authorized")]
    Authorized,
    [Description("Declined")]
    Declined,
    Rejected
}