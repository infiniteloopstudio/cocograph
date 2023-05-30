using Microsoft.Graph;

namespace cocograph.Domain.Services;

/// <summary>
///		A service for working with MS Graph.
/// </summary>
public class MsGraphService
{
	/* Dependencies */
	private readonly IConfiguration _config;
	private readonly TokenService _tokenService;

	/// <summary>
	///		Default injectable constructor.
	/// </summary>
	public MsGraphService(
		IConfiguration config,
		TokenService tokenService
	)
	{
		_config = config;
		_tokenService = tokenService;
	}

	public async Task<UserProfileResponseModel?> GetUserProfileAsync()
	{
		var user = await (await GetClientAsync())
			.Me
			.Request()
			.Select("city,displayname,givenname,mail,state,streetaddress,surname,userprincipalname,postalcode")
			.GetAsync();

		return user is null
			? null
			: new()
			{
				Address = user.StreetAddress,
				City = user.City,
				DisplayName = user.DisplayName,
				GivenName = user.GivenName,
				Mail = user.Mail,
				Photo = await GetUserPhotoDataAsync(),
				State = user.State,
				Surname = user.Surname,
				UserPrincipalName = user.UserPrincipalName,
				ZipCode = user.PostalCode,
			};
	}

	// ------------------------------------------------------------------------------------------------------------------------------------
	// Internals
	// ------------------------------------------------------------------------------------------------------------------------------------
	
	private async Task<GraphServiceClient> GetClientAsync()
	{
		var token = await _tokenService.GetGraphTokenForUserAsync();
		return new(
			baseUrl: _config.GetValue<string>("Graph:BaseUrl"),
			authenticationProvider: new DelegateAuthenticationProvider(
				request =>
				{
					request.Headers.Authorization = new("bearer", token);
					return Task.CompletedTask;
				}));
	}
	
	private async Task<string?> GetUserPhotoDataAsync()
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
		return await Task.FromResult(photoData);
	}
}
