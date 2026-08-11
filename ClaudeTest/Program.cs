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

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services
    .AddScoped<ITeamsService, TeamsService>()
    .AddScoped<IGamesService, GamesService>();

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
