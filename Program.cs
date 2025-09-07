using Calendar.Components;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddGoogle(options =>
    {
        var googleConfig = builder.Configuration.GetRequiredSection("Authentication:Google");
        options.ClientId = googleConfig["ClientId"]  ?? throw new InvalidOperationException("Google ClientId is missing in configuration.");
        options.ClientSecret = googleConfig["ClientSecret"] ?? throw new InvalidOperationException("Google ClientSecret is missing in configuration.");
        options.Scope.Add("email");
        options.ClaimActions.MapJsonKey(System.Security.Claims.ClaimTypes.Email, "email");
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapGet("/login", async httpContext =>
{
    var props = new AuthenticationProperties { RedirectUri = "/" };
    await httpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, props);
});

app.MapGet("/logout", async httpContext =>
{
    await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    httpContext.Response.Redirect("/");
});

app.Run();