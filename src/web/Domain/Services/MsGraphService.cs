using Microsoft.Graph;

namespace cocograph.Domain.Services;

/// <summary>
///		A service for working with MS Graph.
/// </summary>
internal sealed class MsGraphService
{
	/* Dependencies */
	private readonly IConfiguration _config;
	private readonly ITokenAcquisition _tokenAcquisition;

	/// <summary>
	///		Default injectable constructor.
	/// </summary>
	public MsGraphService(IConfiguration config, ITokenAcquisition tokenAcquisition)
	{
		_config = config;
		_tokenAcquisition = tokenAcquisition;
	}

	/// <summary>
	///		Get the current user's profile.
	/// </summary>
	/// <returns></returns>
	public async Task<UserProfile?> GetCurrentUserProfileAsync()
	{
		var user = await (await GetGraphClientAsync().ConfigureAwait(false))
			.Me
			.Request()
			.Select("city,displayname,givenname,id,mail,state,streetaddress,surname,userprincipalname,postalcode")
			.GetAsync()
			.ConfigureAwait(false);

		return user is null
			? null
			: new()
			{
				Address = user.StreetAddress,
				City = user.City,
				DisplayName = user.DisplayName,
				GivenName = user.GivenName,
				Mail = user.Mail,
				ObjectId = user.Id,
				Photo = await GetCurrentUserPhotoDataAsync()
					.ConfigureAwait(false),
				State = user.State,
				Surname = user.Surname,
				UserPrincipalName = user.UserPrincipalName,
				ZipCode = user.PostalCode,
			};
	}

	// ------------------------------------------------------------------------------------------------------------------------------------
	// Internals
	// ------------------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	///		Gets the MS Graph client, preloaded with the delegated authentication token.
	/// </summary>
	private async Task<GraphServiceClient> GetGraphClientAsync()
	{
		var token = await GetDelegatedGraphTokenForCurrentUserAsync().ConfigureAwait(false);
		return new(
			baseUrl: _config.GetValue<string>("Graph:BaseUrl"),
			authenticationProvider: new DelegateAuthenticationProvider(
				request =>
				{
					request.Headers.Authorization = new("bearer", token);
					return Task.CompletedTask;
				}));
	}

	/// <summary>
	///		Acquires a delegated graph token for the current user.
	/// </summary>
	private async Task<string> GetDelegatedGraphTokenForCurrentUserAsync() => await _tokenAcquisition
		.GetAccessTokenForUserAsync(_config.GetValue<string>("Graph:Scopes")?.Split(' ') ?? Array.Empty<string>())
		.ConfigureAwait(false);

	/// <summary>
	///		Gets the current user's photo data.
	/// </summary>
	private async Task<string?> GetCurrentUserPhotoDataAsync()
	{
		string? photoData = null;
		// try
		// {
		// 	await using var photoStream = await client
		// 		.Me
		// 		.Photo
		// 		.Content
		// 		.Request()
		// 		.GetAsync();
		// 	photoData = Convert.ToBase64String(((MemoryStream)photoStream).ToArray());
		// }
		// catch (Exception pex) { Console.WriteLine($"{pex.Message}"); }
		return await Task.FromResult(photoData).ConfigureAwait(false);
	}
}
