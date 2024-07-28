using MediatR;
using InvestmentManager.Shared.Utilities.Abstractions.Result;
using Api.FinancialProduct.Domain.Errors;
using Api.FinancialProduct.Domain.Interfaces.Repositories;

namespace Api.FinancialProduct.Application.Products.Commands.Delete;

public class DeleteProductCommandHandler(IProductRepository productRepository) 
    : IRequestHandler<DeleteProductCommand, Result>
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var productExists = (await _productRepository.GetByIdAsync(request.Id)) != null;

        if (!productExists)
        {
            return Result.Failure(ProductErrors.NotFound);
        }

        await _productRepository.DeleteByIdAsync(request.Id);

        return Result.Success();
    }
}
