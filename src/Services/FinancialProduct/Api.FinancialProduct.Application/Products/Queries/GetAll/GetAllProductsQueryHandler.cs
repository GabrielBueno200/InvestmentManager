using MediatR;
using Api.FinancialProduct.Application.Mapping;
using InvestmentManager.Shared.Utilities.Abstractions;
using InvestmentManager.Shared.Utilities.Abstractions.Result;
using InvestmentManager.Shared.Utilities.Contracts.Dtos;
using System.Net;
using Api.FinancialProduct.Domain.Interfaces.Repositories;

namespace Api.FinancialProduct.Application.Products.Queries.GetAll;

public class GetProductByIdQueryHandler(IProductRepository productRepository) 
    : IRequestHandler<GetAllProductsQuery, Result<PaginatedResult<ProductResponseDto>>>
{
    private readonly IProductRepository _productRepository = productRepository;
    
    public async Task<Result<PaginatedResult<ProductResponseDto>>> Handle(
        GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAllAsync(request.PageSize, request.LastId);

        var mappedProducts = products.ToResponseDto();

        if (!products.Items.Any())
        {
            return Result.Success(mappedProducts, HttpStatusCode.NoContent);
        }

        return mappedProducts;
    }
}
