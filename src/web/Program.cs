var builder = WebApplication.CreateBuilder(args);

builder
	.Services
	.Tee(services =>
	{
		services
			.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
			.AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));
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
	.AddSingleton<WeatherForecastService>();

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
			.UseRouting();

		app.MapControllers();
		app.MapBlazorHub();
		app.MapFallbackToPage("/_Host");
	})
	.RunAsync();
