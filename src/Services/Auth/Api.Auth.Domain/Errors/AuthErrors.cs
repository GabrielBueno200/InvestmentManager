using System.Net;
using InvestmentManager.Shared.Utilities.Abstractions.Result;

namespace Api.Auth.Domain.Errors;

public static class AuthErrors
{
    public static readonly Error InvalidCredentials = new(
        "InvalidCredentials",
        "Incorrect email or password was provided",
        HttpStatusCode.Unauthorized);

    public static readonly Error UserAlreadyExists = new(
        "UserAlreadyExists",
        "User with provided email already exists",
        HttpStatusCode.Unauthorized);

    public static readonly Error UserNotFound = new(
        "User.NotFound",
        "User with provided data was not found",
        HttpStatusCode.NotFound);
}
