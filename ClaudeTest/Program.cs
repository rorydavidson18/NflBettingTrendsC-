using Microsoft.EntityFrameworkCore;
using ClaudeTest.Data;
using ClaudeTest.Interfaces;
using ClaudeTest.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddAntiforgery();

builder.Services.AddDbContext<NflDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddScoped<ITeamsService, TeamsService>()
    .AddScoped<IGamesService, GamesService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAntiforgery();
app.MapRazorPages();

app.Run();
