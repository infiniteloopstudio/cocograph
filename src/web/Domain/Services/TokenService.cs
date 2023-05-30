namespace cocograph.Domain.Services;

/// <summary>
///		A service for working with tokens.
/// </summary>
public class TokenService
{
	/* Dependencies */
	private readonly IConfiguration _config;
	private readonly ITokenAcquisition _tokenAcquisition;

	/// <summary>
	///		Default injectable constructor.
	/// </summary>
	public TokenService(
		ITokenAcquisition tokenAcquisition,
		IConfiguration config
	)
	{
		_tokenAcquisition = tokenAcquisition;
		_config = config;
	}

	public async Task<string> GetGraphTokenForUserAsync() => await _tokenAcquisition
		.GetAccessTokenForUserAsync(_config.GetValue<string>("Graph:Scopes")?.Split(' ') ?? Array.Empty<string>());
}
