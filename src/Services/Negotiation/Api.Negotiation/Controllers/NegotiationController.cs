using Api.Negotiation.Application.Dtos.Payloads;
using Api.Negotiation.Application.Interfaces;
using Api.Negotiation.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Negotiation.Controllers;

[ApiController]
[Route("negotiation")]
[Authorize]
public class NegotiationController(ITransactionService transactionService) : ControllerBase
{
    private readonly ITransactionService _transactionService = transactionService;

    [HttpPost("buy")]
    public async Task<IActionResult> BuyProductAsync(InvestmentTransactionPayloadDto payload)
    {
        return Ok(await _transactionService.CreateTransactionAsync(payload, TransactionType.Buy));
    }

    [HttpPost("sell")]
    public async Task<IActionResult> SellProductAsync(InvestmentTransactionPayloadDto payload)
    {
        return Ok(await _transactionService.CreateTransactionAsync(payload, TransactionType.Sell));
    }

    [HttpPost]
    public async Task<IActionResult> GetInvestmentsByProductAsync(
        [FromQuery] string productId,
        [FromQuery] int pageSize,
        [FromQuery] string lastId 
    )
    {
        return Ok(await _transactionService.GetInvestmentsByProductAsync(productId, pageSize, lastId));
    }
}
