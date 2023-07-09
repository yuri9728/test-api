using Test.Infrastructure.DbContexts;
using Test.WebApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PORT")))
{
	builder.WebHost.UseUrls($"http://+:{Environment.GetEnvironmentVariable("PORT")}");
}

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var postgresConnectionString = builder.Configuration.GetConnectionString("PostgreSQL");

builder.Services.AddNpgsql<AppDbContext>(postgresConnectionString, npgsqlOptions
	=> npgsqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapBookEndpoints();

app.Run();
