using System.Net;
using System.Text.Json.Serialization;

namespace InvestmentManager.Shared.Utilities.Abstractions.Result;

public class Result
{
    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    [JsonIgnore]
    public int StatusCode { get; private set; }

    public Error Error { get; private set; }

    protected Result(bool isSuccess, Error error)
    {
        if (!IsValidResult(isSuccess, error))
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    private bool IsValidResult(bool isSuccess, Error error) =>
        isSuccess && error == Error.None ||
        !isSuccess && error != Error.None;

    public static Result Success(HttpStatusCode statusCode = HttpStatusCode.OK) =>
        new Result(true, Error.None)
        {
            StatusCode = (int)statusCode
        };
        
    public static Result<TValue> Success<TValue>(TValue value, HttpStatusCode statusCode = HttpStatusCode.OK) => 
        new Result<TValue>(value, true, Error.None)
        {
            StatusCode = (int)statusCode
        };

    public static Result Failure(Error error) => new(false, error);
    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected internal Result(TValue? value, bool isSuccess, Error error) : base(isSuccess, error) => _value = value;

    public TValue? Value => IsSuccess ? _value : throw new InvalidOperationException("The value of a failure cannot be accessed");

    public static implicit operator Result<TValue>(TValue? value)
    {
        if (value is not null)
        {
            return Success(value);
        }

        return Failure<TValue>(new(
            "Entity.NullObject",
            "Referência de objeto nulo identificada"));
    }
}
