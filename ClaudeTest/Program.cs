using Microsoft.EntityFrameworkCore;
using ClaudeTest.Data;
using ClaudeTest.Interfaces;
using ClaudeTest.Services;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration, "AzureAd");

builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();
builder.Services.AddAntiforgery();

builder.Services.AddDbContext<NflDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"))
    .AddPolicy("BetTracker", policy => policy.RequireAuthenticatedUser());

builder.Services
    .AddScoped<ITeamsService, TeamsService>()
    .AddScoped<IGamesService, GamesService>()
    .AddScoped<IUserBetsService, UserBetsService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();
app.MapRazorPages();
app.MapControllers();

app.Run();
