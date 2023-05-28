using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Identity.Client;

namespace cocograph;

public interface ITokenCacheProvider
{
	void Initialize(TokenCache tokenCache);
}

public class RedisTokenCacheProvider : ITokenCacheProvider
{
	private readonly IDistributedCache _cache;

	public RedisTokenCacheProvider(IDistributedCache cache) => _cache = cache;

	public void Initialize(TokenCache tokenCache)
	{
		tokenCache.SetBeforeAccess(BeforeAccessNotification);
		tokenCache.SetAfterAccess(AfterAccessNotification);
	}

	private void BeforeAccessNotification(TokenCacheNotificationArgs args) => args.TokenCache.DeserializeMsalV3(_cache.Get(args.Account?.HomeAccountId?.Identifier));

	private void AfterAccessNotification(TokenCacheNotificationArgs args)
	{
		if (!args.HasStateChanged) return; // if the access operation did not result in a cache update

		_cache.Set(
			key: args.Account.HomeAccountId.Identifier,
			value: args.TokenCache.SerializeMsalV3(),
			options: new()
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
			});
	}
}
