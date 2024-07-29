using MediatR;
using Api.FinancialProduct.Application.Dtos;
using Api.FinancialProduct.Application.Interfaces;
using Api.FinancialProduct.Application.Mapping;
using Api.FinancialProduct.Domain.Errors;
using Api.FinancialProduct.Domain.Interfaces.Repositories;
using InvestmentManager.Shared.Utilities.Abstractions.Result;
using StackExchange.Redis;
using System.Text.Json;

namespace Api.FinancialProduct.Application.Products.Queries.GetExtract;

public class GetProductExtractQueryHandler(
    IProductRepository productRepository, 
    INegotiationApiClient negotiationApiClient,
    IConnectionMultiplexer muxer)
: IRequestHandler<GetProductExtractQuery, Result<ProductExtractResponseDto>>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly INegotiationApiClient _negotiationApiClient = negotiationApiClient;
    private readonly IDatabase _database = muxer.GetDatabase();
    
    public async Task<Result<ProductExtractResponseDto>> Handle(GetProductExtractQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = GetCacheKey(request);
        var cachedData = await _database.StringGetAsync(cacheKey);

        if (cachedData.HasValue)
        {
            var result = JsonSerializer.Deserialize<ProductExtractResponseDto>(cachedData!);
            return result!;
        }

        var product = await _productRepository.GetByIdAsync(request.ProductId);

        if (product is null)
        {
            return Result.Failure<ProductExtractResponseDto>(ProductErrors.NotFound);
        }

        var transactions = await _negotiationApiClient
            .GetTransactionsByProductAsync(request.ProductId, request.PageSize, request.LastId);

        var extractResponse = new ProductExtractResponseDto()
        {
            Product = product.ToResponseDto(),
            Transactions = transactions
        };

        await _database.StringSetAsync(cacheKey, JsonSerializer.Serialize(extractResponse), TimeSpan.FromMinutes(1));

        return extractResponse;
    }

    private string GetCacheKey(GetProductExtractQuery request) => 
        $"products-extract-{request.ProductId}{request.PageSize}{request.LastId ?? string.Empty}";
}