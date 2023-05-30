using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder
	.Services
	.Tee(services =>
	{
		services
			.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
			.AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
			.EnableTokenAcquisitionToCallDownstreamApi()
			.AddDistributedTokenCaches()
			.AddMicrosoftGraph(builder.Configuration.GetSection("Graph"));
		services.AddStackExchangeRedisCache(options => options.Configuration = builder.Configuration.GetConnectionString("Redis"));
		services.AddAuthorization(options => options.FallbackPolicy = options.DefaultPolicy);
		services
			.AddControllersWithViews()
			.AddMicrosoftIdentityUI()
			.AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);
		services.AddRazorPages();
		services
			.AddHttpContextAccessor()
			.AddServerSideBlazor()
			.AddMicrosoftIdentityConsentHandler();
	})
	.AddSingleton<ITokenCacheProvider, RedisTokenCacheProvider>()
	.AddTransient<TokenService>()
	.AddTransient<MsGraphService>();

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
