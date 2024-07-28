using System.Collections.Specialized;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;

namespace InvestmentManager.Shared.Utilities.Abstractions.Builders;

public class RequestBuilder
{
    private HttpMethod _method { get; set; }

    private string _url { get; set; }

    private object _payload { get; set; }

    private IDictionary<string, string> _headers = new Dictionary<string, string>();

    private NameValueCollection _queryParams = HttpUtility.ParseQueryString(new UriBuilder().Query);

    private List<string> _pathParams = [];

    public static RequestBuilder Post => new RequestBuilder().WithMethod(HttpMethod.Post);

    public static RequestBuilder Put => new RequestBuilder().WithMethod(HttpMethod.Put);

    public static RequestBuilder Get => new RequestBuilder().WithMethod(HttpMethod.Get);

    public static RequestBuilder Delete => new RequestBuilder().WithMethod(HttpMethod.Delete);

    private RequestBuilder WithMethod(HttpMethod method)
    {
        _method = method;
        return this;
    }

    public RequestBuilder WithUrl(string url)
    {
        _url = url;
        return this;
    }

    public RequestBuilder WithPayload(object payload)
    {
        _payload = payload;
        return this;
    }


    public RequestBuilder WithQueryParam<TParamValue>(string paramName, TParamValue paramValue)
    {
        if (_pathParams.Any())
        {
            throw new NotSupportedException("Cannot add query params with path params");
        }

        _queryParams[paramName] = paramValue?.ToString()!;
        return this;
    }

    public RequestBuilder WithPathParam<TParamValue>(TParamValue paramValue)
    {
        if (_queryParams.HasKeys())
        {
            throw new NotSupportedException("Cannot add path params with query params");
        }

        _pathParams.Add(paramValue?.ToString()!);
        return this;
    }

    public RequestBuilder WithHeader(string headerName, string headerValue)
    {
        if (!_headers.ContainsKey(headerName))
        {
            _headers.Add(headerName, headerValue);
        }
        return this;
    }

    public async Task<TResponseType> SendAsync<TResponseType>(HttpClient _client) where TResponseType : class
    {
        return (await SendAsync(_client, typeof(TResponseType)) as TResponseType)!;
    }

    public async Task SendAsync(HttpClient _client) => await SendAsync(_client, null)!;

    private async Task<object?> SendAsync(HttpClient _client, Type? responseType)
    {
        ArgumentNullException.ThrowIfNull(_method);

        var content = _payload is null ? string.Empty : JsonSerializer.Serialize(
            _payload, new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
            });

        if (_queryParams.HasKeys())
        {
            _url = $"{_url}?{_queryParams}";
        }

        if (_pathParams.Any())
        {
            _url = $"{_url}/{string.Join("/", _pathParams)}";
        }

        var request = new HttpRequestMessage(_method, _url)
        {
            Content = new StringContent(content, new MediaTypeHeaderValue("application/json")),
        };

        foreach (var header in _headers)
        {
            request.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        var response = await _client.SendAsync(request);

        if (responseType is null)
        {
            return null;
        };

        return JsonSerializer.Deserialize(
            json: await response.Content.ReadAsStringAsync(),
            returnType: responseType,
            options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );
    }
}