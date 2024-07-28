using System.Net;
using InvestmentManager.Shared.Utilities.Abstractions.Result;

namespace Api.Negotiation.Domain.Errors;

public static class InvestmentErrors
{
    public static readonly Error UnavailableProduct = new(
        "ProductUnavailable",
        "Cannot buy or sell unavailable product",
        HttpStatusCode.BadRequest);
}
