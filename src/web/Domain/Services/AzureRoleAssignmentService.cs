namespace cocograph.Domain.Services;

/// <summary>
///		Service for getting Azure Role Assignments.
/// </summary>
internal sealed class AzureRoleAssignmentService
{
	/* Dependencies */
	private readonly AzureHttpClient _azureHttp;

	/* Private Fields */
	private readonly Dictionary<string, MicrosoftAuthorizationRoleDefinitionProperties> _roleDefinitionCache = new();

	/// <summary>
	///		Default injectable constructor.
	/// </summary>
	public AzureRoleAssignmentService(AzureHttpClient azureHttp) => _azureHttp = azureHttp;

	/// <summary>
	///		Get all the role assignments for a particular user under a specific subscription.
	/// </summary>
	/// <param name="userId">Id (guid) of user to get assignments for.</param>
	/// <param name="subscriptionId">Subscription Id (guid) to get assignments under.</param>
	public async Task<IEnumerable<RoleAssignment>> GetRoleAssignments(string userId, string subscriptionId)
	{
		var response = await _azureHttp
			.WithCredentials()
			.GetAsync($"https://management.azure.com/subscriptions/{subscriptionId}/providers/Microsoft.Authorization/roleAssignments?api-version=2022-04-01&$filter=principalId eq '{userId}'")
			.ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
		{
			var roleAssignments = (await response.Content.ReadFromJsonAsync<MicrosoftAuthorizationRoleAssignmentsResponse>().ConfigureAwait(false))!.Value;
			var roleAssignmentInfos = new List<RoleAssignment>();

			foreach (var assignment in roleAssignments!)
				roleAssignmentInfos.Add(new()
				{
					Scope = assignment.Properties!.Scope,
					RoleProperties = await GetRolePropertiesAsync(assignment.Properties.RoleDefinitionId).ConfigureAwait(false)
				});

			return roleAssignmentInfos;
		}

		var error = $"Error: {response.StatusCode}\n{await response.Content.ReadAsStringAsync().ConfigureAwait(false)}";
		Console.WriteLine(error);
		return Array.Empty<RoleAssignment>();
	}

	// ------------------------------------------------------------------------------------------------------------------------------------
	// Internals
	// ------------------------------------------------------------------------------------------------------------------------------------

	/// <summary>
	///		Gets the definition properties of a particular role assignment.
	/// </summary>
	/// <param name="roleDefinitionId">Id of role definition to get properties for.</param>
	/// <exception cref="Exception">When the management API returns a non-success status.</exception>
	private async Task<MicrosoftAuthorizationRoleDefinitionProperties?> GetRolePropertiesAsync(string? roleDefinitionId)
	{
		if (string.IsNullOrEmpty(roleDefinitionId)) return null;
		if (_roleDefinitionCache.TryGetValue(roleDefinitionId, out var role)) return role;

		var url = $"https://management.azure.com{roleDefinitionId}?api-version=2022-04-01";
		var response = await _azureHttp.WithCredentials().GetAsync(url).ConfigureAwait(false);

		if (response.IsSuccessStatusCode)
			return _roleDefinitionCache[roleDefinitionId] = (await response.Content.ReadFromJsonAsync<MicrosoftAuthorizationRoleDefinitionResponse>().ConfigureAwait(false))!.Properties!;

		throw new($"Error fetching role definition: {response.StatusCode}");
	}
}
