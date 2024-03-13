using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserManagement.Services.Domain.Implementations;
using UserManagement.Services.Domain.Interfaces;
using Westwind.AspNetCore.Markdown;
using UserManagement.Data;
using Microsoft.Extensions.Configuration;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddDomainServices()
    .AddMarkdown()
    .AddControllersWithViews();

IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

string? connectionString = configuration.GetConnectionString("DevelopmentConnection");

if (string.IsNullOrEmpty(connectionString))
{
    // Use in-memory database if connection string is not found
    builder.Services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase("InMemoryDatabase"));
}
else
{
    builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));
}

builder.Services.AddScoped<IDataContext, DataContext>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

app.UseMarkdown();

app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();
