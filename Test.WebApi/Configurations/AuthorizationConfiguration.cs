using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Test.WebApi.Configurations;

public static class AuthorizationExtensions
{
	public static IServiceCollection ConfigureAuthorization(this IServiceCollection services)
	{
		var fallbackPolicy = new AuthorizationPolicyBuilder()
			.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
			.RequireAuthenticatedUser()
			.Build();

		services.AddAuthorization(options =>
		{
			options.FallbackPolicy = fallbackPolicy;
		});

		return services;
	}
}
