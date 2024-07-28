using MediatR;
using InvestmentManager.Shared.Utilities.Contracts.Dtos;
using InvestmentManager.Shared.Utilities.Abstractions.Result;
using Api.FinancialProduct.Domain.Errors;
using Api.FinancialProduct.Application.Mapping;
using Api.FinancialProduct.Domain.Interfaces.Repositories;

namespace Api.FinancialProduct.Application.Products.Queries.GetById;

public class GetProductByIdQueryHandler(IProductRepository productRepository) 
    : IRequestHandler<GetProductByIdQuery, Result<ProductResponseDto>>
{
    private readonly IProductRepository _productRepository = productRepository;
    
    public async Task<Result<ProductResponseDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id);

        if (product is null)
        {
            return Result.Failure<ProductResponseDto>(ProductErrors.NotFound);
        }

        return product.ToResponseDto();
    }
}