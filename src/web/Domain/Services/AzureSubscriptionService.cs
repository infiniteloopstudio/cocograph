using Azure.Identity;
using Azure.ResourceManager;

namespace cocograph.Domain.Services;

/// <summary>
///		A service for getting Azure Subscriptions.
/// </summary>
internal sealed class AzureSubscriptionService
{
	/// <summary>
	///		Gets all subscriptions.
	/// </summary>
	public async Task<IEnumerable<Subscription>> GetSubscriptionsAsync()
	{
		var client = new ArmClient(new DefaultAzureCredential());
		var subs = new List<Subscription>();
		
		await foreach (var sub in client.GetSubscriptions().GetAllAsync().ConfigureAwait(false))
			subs.Add(new()
			{
				Name = sub.Data.DisplayName,
				SubscriptionId = sub.Data.SubscriptionId
			});

		return subs;
	}
}
