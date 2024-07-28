using MediatR;
using Api.FinancialProduct.Application.Mapping;
using Api.FinancialProduct.Domain.Errors;
using InvestmentManager.Shared.Utilities.Abstractions.Result;
using InvestmentManager.Shared.Utilities.Contracts.Dtos;
using FluentValidation;
using Api.FinancialProduct.Domain.Interfaces.Repositories;
using InvestmentManager.Shared.Utilities.Helpers;

namespace Api.FinancialProduct.Application.Products.Commands.Update;

public class UpdateProductCommandHandler(IProductRepository productRepository, IValidator<UpdateProductCommand> validator)
    : IRequestHandler<UpdateProductCommand, Result<ProductResponseDto>>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IValidator<UpdateProductCommand> _validator = validator;
    
    public async Task<Result<ProductResponseDto>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var validation = _validator.Validate(request);

        if (!validation.IsValid)
        {
            return validation.GetValidationErrors<ProductResponseDto>();
        }

        var existingProduct = await _productRepository.GetByIdAsync(request.Id);

        if (existingProduct is null)
        {
            return Result.Failure<ProductResponseDto>(ProductErrors.NotFound);
        }

        var updatedProduct = request.ToProduct();

        var hasPriceChanged = existingProduct.Price != updatedProduct.Price;

        updatedProduct.PriceHistory = hasPriceChanged 
            ? [..existingProduct.PriceHistory, updatedProduct.Price]
            : existingProduct.PriceHistory;

        await _productRepository.UpdateAsync(updatedProduct);

        return updatedProduct.ToResponseDto();
    }
}