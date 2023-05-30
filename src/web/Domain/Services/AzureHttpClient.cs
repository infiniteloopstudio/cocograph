using Azure.Core;
using Azure.Identity;

namespace cocograph.Domain.Services;

/// <summary>
///		Reusable http client wrapper that automatically authenticates requests.
/// </summary>
internal sealed class AzureHttpClient
{
	/* Dependencies */
	private readonly HttpClient _httpClient;

	/* Private Fields */
	private readonly DefaultAzureCredential _defaultAzureCredential;
	private AccessToken? _token;
		
	/// <summary>
	///		Default injectable constructor.
	/// </summary>
	public AzureHttpClient(IHttpClientFactory httpClientFactory)
	{
		_httpClient = httpClientFactory.CreateClient();
		_defaultAzureCredential = new();
	}

	/// <summary>
	///		Use the Http Client pre-loaded with the appropriate authentication token.
	/// </summary>
	public HttpClient WithCredentials()
	{
		if (_token is null || _token?.ExpiresOn <= DateTimeOffset.Now)
			_token = _defaultAzureCredential.GetTokenAsync(new(new[] { "https://management.azure.com/.default" })).Result;

		_httpClient.DefaultRequestHeaders.Authorization = new("Bearer", _token?.Token);
		return _httpClient;
	}
}
