namespace cocograph.Domain.Models;

public class UserProfileResponseModel
{
	public string DisplayName { get; init; }
	public string Surname { get; init; }
	public string GivenName { get; init; }
	public string UserPrincipalName { get; init; }
	public string? Photo { get; init; }
}
