using Microsoft.AspNetCore.Mvc;

using PaymentGateway.Api.Interfaces;
using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Models.Responses;

namespace PaymentGateway.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentsController(IPaymentService paymentService) : Controller
{
    [HttpPost]
    [ProducesResponseType<PostPaymentResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PostPaymentResponse>> ProcessPaymentsAsync([FromBody] PostPaymentRequest request)
    {
        var payment = await paymentService.SendPaymentRequestAsync(request);
        return new OkObjectResult(payment);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PostPaymentResponse?>> GetPaymentAsync(Guid id)
    {
        var payment = await paymentService.GetPaymentDataAsync(id);
        return payment is not null ? new OkObjectResult(payment) : new NotFoundResult();
    }
}