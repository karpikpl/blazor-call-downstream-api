using System.Security.Claims;
using Microsoft.Identity.Abstractions;

public class ApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IAuthorizationHeaderProvider _authorizationHeaderProvider;
    private readonly IConfiguration _configuration;
    private readonly Microsoft.Identity.Abstractions.IDownstreamApi _downstreamApi;

    public ApiClient(HttpClient httpClient,
        IAuthorizationHeaderProvider authorizationHeaderProvider,
        IConfiguration configuration,
        Microsoft.Identity.Abstractions.IDownstreamApi downstreamApi)
    {
        _httpClient = httpClient;
        _authorizationHeaderProvider = authorizationHeaderProvider;
        _configuration = configuration;
        _downstreamApi = downstreamApi;


    }

    private class FooResponse
    {
        public string Foo { get; set; }
    }

    public async Task<string> GetAsync(ClaimsPrincipal? user)
    {
        if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
        {
            var scopes = _configuration["MyApi:Scopes"]?.Split(' ');

            // After the token has been returned by Microsoft.Identity.Web, add it to the HTTP authorization header before making the call to access the todolist service.
            var authorizationHeader = await _authorizationHeaderProvider.CreateAuthorizationHeaderForUserAsync(scopes, claimsPrincipal: user);
            _httpClient.DefaultRequestHeaders.Add("Authorization", authorizationHeader);
        }

        var data = await _httpClient.GetFromJsonAsync<FooResponse>("Foo");

        return data.Foo;
    }
}