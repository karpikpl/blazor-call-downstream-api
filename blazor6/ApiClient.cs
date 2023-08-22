using System.Security.Claims;

public interface IApiClient
{
    Task<string> GetDataHttpClientAsync(ClaimsPrincipal? user);
    Task<string> GetDataApiDownstreamAsync();
}

public class ApiClient: IApiClient 
{
    private readonly HttpClient _httpClient;
    private readonly Microsoft.Identity.Abstractions.IAuthorizationHeaderProvider _authorizationHeaderProvider;
    private readonly IConfiguration _configuration;
    private readonly Microsoft.Identity.Abstractions.IDownstreamApi _downstreamApi;
    private readonly Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider _authenticationStateProvider;

    const string ServiceName = "MyApi";
    const string RequestPath = "Foo";

    public ApiClient(HttpClient httpClient,
        Microsoft.Identity.Abstractions.IAuthorizationHeaderProvider authorizationHeaderProvider,
        IConfiguration configuration,
        Microsoft.Identity.Abstractions.IDownstreamApi downstreamApi,
         Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider authenticationStateProvider)
    {
        _httpClient = httpClient;
        _authorizationHeaderProvider = authorizationHeaderProvider;
        _configuration = configuration;
        _downstreamApi = downstreamApi;
        _authenticationStateProvider = authenticationStateProvider;
    }

    private class FooResponse
    {
        public string Foo { get; set; }
    }

    public async Task<string> GetDataHttpClientAsync(ClaimsPrincipal? user)
    {
        if (user == null)
        {
            var authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync().ConfigureAwait(false);
            user = authenticationState.User;
        }

        if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
        {
            var scopes = _configuration[$"{ServiceName}:Scopes"]?.Split(' ');

            // After the token has been returned by Microsoft.Identity.Web, add it to the HTTP authorization header before making the call to access the todolist service.
            var authorizationHeader = await _authorizationHeaderProvider.CreateAuthorizationHeaderForUserAsync(scopes, claimsPrincipal: user);
            _httpClient.DefaultRequestHeaders.Add("Authorization", authorizationHeader);
        }

        var data = await _httpClient.GetFromJsonAsync<FooResponse>(RequestPath);
        return data.Foo;
    }

    public async Task<string> GetDataApiDownstreamAsync()
    {
        var principal = await _authenticationStateProvider.GetAuthenticationStateAsync();

        var value = await _downstreamApi.CallApiForUserAsync<FooResponse>(ServiceName, options =>
        {
            options.HttpMethod = HttpMethod.Get;
            options.RelativePath = RequestPath;
        },
        principal.User
        );

        return value.Foo;
    }
}