using System.Net;
using System.Text.Json.Serialization;

namespace InvestmentManager.Shared.Utilities.Abstractions.Result;

public record Error(string Code, string? Description = null)
{
    [JsonIgnore]
    public int StatusCode { get; set; } = (int)HttpStatusCode.BadRequest;

    public Error(
        string Code,
        string? Description = null,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest) : this(Code, Description)
    {
        StatusCode = (int) statusCode;
    }

    public static readonly Error None = new(string.Empty, null);

    public static implicit operator Result(Error error) => Result.Failure(error);
}