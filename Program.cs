using System.Data;
using System.Security.Claims;
using Calendar.Components;
using Calendar.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<IDbConnection>(_ => new Microsoft.Data.SqlClient.SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddTransient<UserService>();

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
        options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");

        options.Events.OnCreatingTicket = async context =>
        {
            var email = context.Principal?.FindFirstValue(ClaimTypes.Email);
            var name = context.Principal?.Identity?.Name;
            if (email == null || name == null)
            {
                Console.WriteLine("An error occured during authentication");
                return;
            }

            using var scope = context.HttpContext.RequestServices.CreateScope();
            var userService = scope.ServiceProvider.GetRequiredService<UserService>();
            var user = await userService.GetUserAsync(email);
            int userId;

            if (user is null)
            {
                userId = await userService.CreateUserAsync(email, name);
            }
            else
            {
                userId = user.Id;
            }
            
            var identity = (ClaimsIdentity)context.Principal?.Identity!;
            identity.AddClaim(new Claim("UserId", userId.ToString()));
        };
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