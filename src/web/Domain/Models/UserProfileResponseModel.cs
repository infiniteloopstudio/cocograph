using Microsoft.Graph;

namespace cocograph.Domain.Models;

public class UserProfileResponseModel
{
	public User User { get; init; }
	public string? Photo { get; init; }
}
