var builder = WebApplication.CreateBuilder(args);

builder
	.Services
	.Tee(services =>
	{
		services
			.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
			.AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
			.EnableTokenAcquisitionToCallDownstreamApi(builder.Configuration.GetValue<string>("DownstreamApi:Scopes")?.Split(' '))
			.AddMicrosoftGraph()
			.AddDistributedTokenCaches();
		services.AddStackExchangeRedisCache(options => options.Configuration = builder.Configuration.GetConnectionString("Redis"));
		services.AddAuthorization(options => options.FallbackPolicy = options.DefaultPolicy);
		services
			.AddControllersWithViews()
			.AddMicrosoftIdentityUI();
		services.AddRazorPages();
		services
			.AddHttpContextAccessor()
			.AddServerSideBlazor()
			.AddMicrosoftIdentityConsentHandler();
	})
	.AddSingleton<ITokenCacheProvider, RedisTokenCacheProvider>()
	.AddTransient<UserGraphService>();

await builder
	.Build()
	.TeeWhen(
		condition: app => !app.Environment.IsDevelopment(),
		action: app => app
			.UseExceptionHandler("/Error")
			.UseHsts())
	.Tee(app =>
	{
		app
			.UseHttpsRedirection()
			.UseStaticFiles()
			.UseRouting()
			.UseAuthentication()
			.UseAuthorization();

		app.MapControllers();
		app.MapBlazorHub();
		app.MapFallbackToPage("/_Host");
	})
	.RunAsync();
