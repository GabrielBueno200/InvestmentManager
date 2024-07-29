using MediatR;
using Microsoft.AspNetCore.Mvc;
using Api.FinancialProduct.Application.Products.Commands.Create;
using Api.FinancialProduct.Application.Products.Commands.Delete;
using Api.FinancialProduct.Application.Products.Commands.Update;
using Api.FinancialProduct.Application.Products.Queries.GetAll;
using Api.FinancialProduct.Application.Products.Queries.GetById;
using Microsoft.AspNetCore.Authorization;
using InvestmentManager.Shared.Utilities.Constants;
using Api.FinancialProduct.Application.Products.Queries.GetExtract;

namespace Api.FinancialProduct.Controllers;

[ApiController]
[Route("products")]
public class ProductController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpPost]
    [Authorize(Roles = $"{Role.Admin}, {Role.Operation}")]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
    {
        return Ok(await _sender.Send(command));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int pageSize, [FromQuery] string? lastId = null)
    {
        return Ok(await _sender.Send(new GetAllProductsQuery(pageSize, lastId)));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        return Ok(await _sender.Send(new GetProductByIdQuery(id)));
    }

    [HttpGet("extract")]
    public async Task<IActionResult> GetExtract(
        [FromQuery] string productId, [FromQuery] int pageSize, [FromQuery] string? lastId = null)
    {
        return Ok(await _sender.Send(new GetProductExtractQuery(productId, pageSize, lastId)));
    }

    [HttpPut]
    [Authorize(Roles = $"{Role.Admin}, {Role.Operation}")]
    public async Task<IActionResult> Update([FromBody] UpdateProductCommand command)
    {
        return Ok(await _sender.Send(command));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = $"{Role.Admin}, {Role.Operation}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        return Ok(await _sender.Send(new DeleteProductCommand(id)));
    }
}
