// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace cocograph.Domain.Models;

internal sealed class UserProfile
{
	public string? Address { get; init; }
	public string? City { get; init; }
	public string? DisplayName { get; init; }
	public string? GivenName { get; init; }
	public string? Mail { get; init; }
	public string? ObjectId { get; init; }
	public string? Photo { get; init; }
	public string? State { get; init; }
	public string? Surname { get; init; }
	public string? UserPrincipalName { get; init; }
	public string? ZipCode { get; init; }
}
