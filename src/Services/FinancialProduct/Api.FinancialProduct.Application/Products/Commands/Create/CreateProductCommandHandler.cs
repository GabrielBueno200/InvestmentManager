using MediatR;
using InvestmentManager.Shared.Utilities.Contracts.Dtos;
using Api.FinancialProduct.Application.Mapping;
using InvestmentManager.Shared.Utilities.Abstractions.Result;
using FluentValidation;
using System.Net;
using Api.FinancialProduct.Domain.Interfaces.Repositories;
using InvestmentManager.Shared.Utilities.Helpers;

namespace Api.FinancialProduct.Application.Products.Commands.Create;

public class CreateProductCommandHandler(IProductRepository productRepository, IValidator<CreateProductCommand> validator) 
    : IRequestHandler<CreateProductCommand, Result<ProductResponseDto>>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IValidator<CreateProductCommand> _validator = validator;

    public async Task<Result<ProductResponseDto>> Handle(
        CreateProductCommand request, CancellationToken cancellationToken)
    {
        var validation = _validator.Validate(request);

        if (!validation.IsValid)
        {
            return validation.GetValidationErrors<ProductResponseDto>();
        }

        var productToCreate = request.ToProduct();
        productToCreate.PriceHistory.Add(productToCreate.Price);

        var createdProduct = await _productRepository.AddAsync(productToCreate);

        return Result.Success(createdProduct.ToResponseDto(), HttpStatusCode.Created);
    }
}