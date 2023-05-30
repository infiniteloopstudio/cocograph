// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace cocograph.Domain.Models;

internal sealed class MicrosoftAuthorizationRoleAssignmentsResponse
{
	public List<MicrosoftAuthorizationRoleAssignment>? Value { get; init; }
}