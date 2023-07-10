using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Test.WebApi.Configurations;

public static class AuthenticationConfiguration
{
	public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
	{
		LifetimeValidator lifetimeValidator = new((
			notBefore,
			expires,
			securityToken,
			validationParameter) => DateTime.UtcNow <= expires);

		string? key = configuration["Secrets:SecurityKey"];
		ArgumentException.ThrowIfNullOrEmpty(key);

		TokenValidationParameters tokenValidationParameters = new()
		{
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
			ValidateIssuer = true,
			ValidIssuer = Environment.MachineName,
			ValidateAudience = false,
			ValidateLifetime = true,
			LifetimeValidator = lifetimeValidator
		};

		services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
		{
			options.TokenValidationParameters = tokenValidationParameters;
		});

		return services;
	}
}
