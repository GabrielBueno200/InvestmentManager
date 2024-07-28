using FluentValidation.Results;
using InvestmentManager.Shared.Utilities.Abstractions.Result;

namespace InvestmentManager.Shared.Utilities.Helpers;

public static class ValidationHelpers
{
    public static Result<TEntity> GetValidationErrors<TEntity>(this ValidationResult result)
    {
        var errorMessage = string.Join(", ", result.Errors.Select(error => error.ErrorMessage));

        return Result.Failure<TEntity>(new Error("ValidationError", errorMessage));
    }
}
