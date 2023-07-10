using Microsoft.EntityFrameworkCore;
using Test.Application.Mappers;
using Test.Domain;
using Test.Infrastructure.DbContexts;
using Test.WebApi.Configurations;
using Test.WebApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// For Yandex Cloud deployment
string? port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
	builder.WebHost.UseUrls($"http://+:{port}");
}

string? postgresConnectionString = builder.Configuration.GetConnectionString("PostgreSQL");
builder.Services.AddNpgsql<AppDbContext>(postgresConnectionString, npgsqlOptions
	=> npgsqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));

builder.Services.AddTransient<BookMapper>();

builder.Services.ConfigureOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapBookEndpoints().MapAuthenticationEndpoints();

app.Run();
