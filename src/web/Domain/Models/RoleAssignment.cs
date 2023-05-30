// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace cocograph.Domain.Models;

internal sealed class RoleAssignment
{
	public string? Scope { get; init; }
	public MicrosoftAuthorizationRoleDefinitionProperties? RoleProperties { get; init; }
}
