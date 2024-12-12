using FluentValidation;

using PaymentGateway.Api.Models.Requests;

namespace PaymentGateway.Api.Validators;

public class PostPaymentRequestValidator : AbstractValidator<PostPaymentRequest>
{
    private readonly IEnumerable<string> _isoCodes = ["usd", "brl", "gbp"];

    public PostPaymentRequestValidator()
    {
        RuleFor(p => p.CardNumber)
           .Matches(@"^\d{14,19}$")
           .WithMessage("Card number should only be digits and contain between 14-19 digits.");
        When(p => p.ExpiryYear == DateTime.Now.Year && p.ExpiryMonth == 12, () =>
        {
            RuleFor(p => p.ExpiryYear)
               .GreaterThan(DateTime.Now.Year)
               .WithMessage("Expiry year should be in the future");
        });

        When(p => p.ExpiryYear == DateTime.Now.Year, () =>
            {
                RuleFor(p => p.ExpiryMonth)
                   .InclusiveBetween(DateTime.Now.Month, 12)
                   .WithMessage("Expiry month should be int the future.");
            })
           .Otherwise(() =>
            {
                RuleFor(p => p.ExpiryMonth)
                   .InclusiveBetween(1, 12);
                RuleFor(p => p.ExpiryYear)
                   .GreaterThanOrEqualTo(DateTime.Now.Year);
            });

        RuleFor(p => p.Currency)
           .Must(isoCode => _isoCodes.Contains(isoCode.ToLower()))
           .WithMessage("Currency should be one of the following: USD, BRL, EUR");
        RuleFor(p => p.Amount)
           .GreaterThan(0);
        RuleFor(p => p.Cvv.ToString())
           .Matches(@"\d{3,4}")
           .WithMessage("CVV should contain between 3-4 digits.");
    }
}