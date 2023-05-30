// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace cocograph.Domain.Models;

internal sealed class MicrosoftAuthorizationRoleAssignment
{
	public MicrosoftAuthorizationRoleAssignmentProperties? Properties { get; init; }
	public string? Id { get; init; }
	public string? Type { get; init; }
	public string? Name { get; init; }
}