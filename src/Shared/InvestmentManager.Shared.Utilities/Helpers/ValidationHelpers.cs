using FluentValidation.Results;
using InvestmentManager.Shared.Utilities.Abstractions.Result;

namespace InvestmentManager.Shared.Utilities.Helpers;

public static class ValidationHelpers
{
    public static Result<TEntity> GetValidationErrors<TEntity>(this ValidationResult result)
    {
        return Result.Failure<TEntity>(new Error("ValidationError", GetErrorMessages(result)));
    }

    public static Result GetValidationErrors(this ValidationResult result)
    {
        return Result.Failure(new Error("ValidationError", GetErrorMessages(result)));
    }

    private static string GetErrorMessages(ValidationResult result) 
    {
        return string.Join(", ", result.Errors.Select(error => error.ErrorMessage));
    }
}
