using Microsoft.EntityFrameworkCore;
using Test.Application.Mappers;
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
builder.Services.AddTransient<UserMapper>();
builder.Services.AddTransient<AuthorMapper>();

builder.Services
	.ConfigureAuthentication(builder.Configuration)
	.ConfigureAuthorization()
	.ConfigureOpenApi();

var app = builder.Build();

app.UseSwagger();
if (app.Environment.IsDevelopment())
{
	app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Test API").AllowAnonymous();

app.MapAuthenticationEndpoints()
	.MapUserEndpoints()
	.MapAuthorEndpoints()
	.MapBookEndpoints();

app.Run();
