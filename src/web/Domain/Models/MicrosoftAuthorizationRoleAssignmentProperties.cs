// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace cocograph.Domain.Models;

internal sealed class MicrosoftAuthorizationRoleAssignmentProperties
{
	public string? RoleDefinitionId { get; init; }
	public string? PrincipalId { get; init; }
	public string? PrincipalType { get; init; }
	public string? Scope { get; init; }
	public string? Condition { get; init; }
	public string? ConditionVersion { get; init; }
	public DateTime? CreatedOn { get; init; }
	public DateTime? UpdatedOn { get; init; }
	public string? CreatedBy { get; init; }
	public string? UpdatedBy { get; init; }
	public string? DelegatedManagedIdentityResourceId { get; init; }
	public string? Description { get; init; }
}