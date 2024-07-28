using System.Net;
using InvestmentManager.Shared.Utilities.Abstractions.Result;

namespace Api.FinancialProduct.Domain.Errors;

public static class ProductErrors
{
    public static readonly Error NotFound = new(
        "NotFound",
        "No product with the given Id was found",
        HttpStatusCode.NotFound);
}
