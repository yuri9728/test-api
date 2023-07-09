using Microsoft.OpenApi.Models;

namespace Test.WebApi.Configurations;

public static class OpenApiConfiguration
{
	public static IServiceCollection ConfigureOpenApi(this IServiceCollection services)
	{
		OpenApiInfo openApiInfo = new()
		{
			Title = "Test API",
			Version = "v 0.0.1",
			Description = "API for test task"
		};

		OpenApiSecurityScheme openApiSecurityScheme = new()
		{
			In = ParameterLocation.Header,
			Description = "Please enter token",
			Name = "Authorization",
			Type = SecuritySchemeType.Http,
			BearerFormat = "JWT",
			Scheme = "bearer"
		};

		OpenApiSecurityRequirement openApiSecurityRequirement = new()
		{
			{
				new OpenApiSecurityScheme
				{
					Reference = new OpenApiReference
					{
						Type = ReferenceType.SecurityScheme,
						Id = "Bearer"
					}
				},
				Array.Empty<string>()
			}
		};

		services
			.AddEndpointsApiExplorer()
			.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", openApiInfo);
				options.AddSecurityDefinition("Bearer", openApiSecurityScheme);
				options.AddSecurityRequirement(openApiSecurityRequirement);
			});

		return services;
	}
}
