using MediatR;
using System.Net;
using InvestmentManager.Shared.Utilities.Abstractions;
using InvestmentManager.Shared.Utilities.Contracts.Dtos;
using InvestmentManager.Shared.Utilities.Abstractions.Result;
using Api.FinancialProduct.Application.Products.Queries.GetAll;
using Api.FinancialProduct.Application.Mapping;
using Api.FinancialProduct.Domain.Interfaces.Repositories;

namespace Api.FinancialProduct.Application.Products.Queries.GetById;

public class GetAvailableProductsQueryHandler(IProductRepository productRepository) 
    : IRequestHandler<GetAvailableProductsQuery, Result<PaginatedResult<ProductResponseDto>>>
{
    private readonly IProductRepository _productRepository = productRepository;
    
    public async Task<Result<PaginatedResult<ProductResponseDto>>> Handle(
        GetAvailableProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAvailableProductsAsync(request.PageSize, request.LastId);

        var mappedProducts = products.ToResponseDto();

        if (!products.Items.Any())
        {
            return Result.Success(mappedProducts, HttpStatusCode.NoContent);
        }

        return mappedProducts;
    }
}