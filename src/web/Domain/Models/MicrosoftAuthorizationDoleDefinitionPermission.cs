namespace cocograph.Domain.Models;

internal sealed class MicrosoftAuthorizationDoleDefinitionPermission
{
	public List<string>? Actions { get; set; }
	public List<string>? NotActions { get; set; }
	public List<object>? DataActions { get; set; }
	public List<object>? NotDataActions { get; set; }
}