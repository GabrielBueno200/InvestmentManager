using InvestmentManager.Shared.Utilities.Abstractions.Result;
using MediatR;

namespace Api.FinancialProduct.Application.Products.Commands.Delete;

public record DeleteProductCommand(string Id) : IRequest<Result>;
