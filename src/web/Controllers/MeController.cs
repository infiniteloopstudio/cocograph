// using cocograph.Domain.Services;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Graph;
// using Microsoft.Identity.Client;
//
// namespace cocograph.Controllers;
//
// [Authorize]
// public class MeController : Controller
// {
// 	/* Dependencies */
// 	private readonly MicrosoftIdentityConsentAndConditionalAccessHandler _consentHandler;
// 	private string[] _graphScopes;
// 	private readonly ILogger<MeController> _logger;
// 	private readonly UserGraphService _userGraphService;
// 	
// 	/// <summary>
// 	///		Default injectable constructor.
// 	/// </summary>
// 	public MeController(
// 		IConfiguration config,
// 		MicrosoftIdentityConsentAndConditionalAccessHandler consentHandler,
// 		ILogger<MeController> logger,
// 		UserGraphService userGraphService
// 	)
// 	{
// 		_consentHandler = consentHandler;
// 		_logger = logger;
// 		_userGraphService = userGraphService;
// 		_graphScopes = config.GetValue<string>("DownstreamApi:Scopes")?.Split(' ') ?? Array.Empty<string>();
// 	}
// 	
// 	[AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
// 	public async Task<IActionResult> Profile()
// 	{
// 		UserProfileResponseModel currentUser = null;
//
// 		try
// 		{
// 			currentUser = await _userGraphService.GetCurrentUserProfileAsync();
// 		}
// 		// Catch CAE exception from Graph SDK
// 		catch (ServiceException svcex) when (svcex.Message.Contains("Continuous access evaluation resulted in claims challenge"))
// 		{
// 			try
// 			{
// 				Console.WriteLine($"{svcex}");
// 				var claimChallenge = WwwAuthenticateParameters.GetClaimChallengeFromResponseHeaders(svcex.ResponseHeaders);
// 				_consentHandler.ChallengeUser(_graphScopes, claimChallenge);
// 				return NoContent();
// 			}
// 			catch (Exception ex2)
// 			{
// 				_consentHandler.HandleException(ex2);
// 			}
// 		}
//
// 		if (currentUser is null) NotFound();
// 		return Ok(currentUser);
// 	}
// }
