// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace cocograph.Domain.Models;

internal sealed class MicrosoftAuthorizationRoleDefinitionProperties
{
	public string? RoleName { get; set; }
	public string? Type { get; set; }
	public string? Description { get; set; }
	public List<string>? AssignableScopes { get; set; }
	public List<MicrosoftAuthorizationDoleDefinitionPermission>? Permissions { get; set; }
	public DateTime? CreatedOn { get; set; }
	public DateTime? UpdatedOn { get; set; }
	public string? CreatedBy { get; set; }
	public string? UpdatedBy { get; set; }
}