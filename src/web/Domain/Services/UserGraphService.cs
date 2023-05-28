using Microsoft.Graph;

namespace cocograph.Domain.Services;

public class UserGraphService
{
	/* Dependencies */
	private readonly GraphServiceClient _graphServiceClient;
	
	/// <summary>
	///		Default injectable constructor.
	/// </summary>
	public UserGraphService(GraphServiceClient graphServiceClient) => _graphServiceClient = graphServiceClient;

	public async Task<UserProfileResponseModel?> GetCurrentUserProfileAsync()
	{
		var currentUser = await _graphServiceClient.Me.Request().GetAsync();
		if (currentUser is null) return null;
		
		/*
		 * Get user photo
		 */
		string? photoData = null;
		try
		{
			await using var photoStream = await _graphServiceClient.Me.Photo.Content.Request().GetAsync();
			photoData = Convert.ToBase64String(((MemoryStream)photoStream).ToArray());
		}
		catch (Exception pex)
		{
			Console.WriteLine($"{pex.Message}");
		}
		
		return new()
		{
			User = currentUser,
			Photo = photoData
		};
	}
}
