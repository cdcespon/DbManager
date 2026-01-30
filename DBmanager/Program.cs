using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using DBmanager.Data;
using DBmanager.Services; // Added this using directive for BackupService, SqliteService, AppState

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<Radzen.DialogService>();
builder.Services.AddScoped<Radzen.NotificationService>();
builder.Services.AddScoped<Radzen.TooltipService>();
builder.Services.AddScoped<Radzen.ContextMenuService>();
builder.Services.AddScoped<SqliteService>(); // Changed to use 'using DBmanager.Services;'
builder.Services.AddScoped<AppState>();
builder.Services.AddScoped<SessionService>(); // Changed to use 'using DBmanager.Services;'
builder.Services.AddScoped<BackupService>(); // Added BackupService

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
