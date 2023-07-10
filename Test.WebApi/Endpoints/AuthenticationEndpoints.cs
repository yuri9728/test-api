using IdentityModel;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Test.Application.DTOs;
using Test.Domain;
using Test.Infrastructure.DbContexts;

namespace Test.WebApi.Endpoints;

public static class AuthenticationEndpoints
{
	public static IEndpointRouteBuilder MapAuthenticationEndpoints(this IEndpointRouteBuilder routeBuilder)
	{
		var group = routeBuilder.MapGroup("/authentication")
			.WithTags("Authentication")
			.AllowAnonymous()
			.WithOpenApi();

		group.MapPost("/login", Login)
			.WithName(nameof(Login));

		return routeBuilder;
	}

	private static async Task<Results<Ok<string>, UnauthorizedHttpResult>> Login(
		AppDbContext db,
		IConfiguration configuration,
		LoginRequest loginRequest)
	{
		User? loggingInUser = await db.Users
			.FirstOrDefaultAsync(user => user.Login == loginRequest.Login && user.PasswordHash == loginRequest.Password);

		if (loggingInUser is null) 
		{
			return TypedResults.Unauthorized();
		}

		List<Claim> claims = new()
		{
			new Claim(JwtClaimTypes.Id, loggingInUser.Id.ToString()),
		};

		ClaimsIdentity identity = new(claims);
		ClaimsPrincipal principal = new(identity);

		string? encryptionKey = configuration["Secrets:SecurityKey"];
		ArgumentException.ThrowIfNullOrEmpty(encryptionKey);

		SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(encryptionKey));
		SigningCredentials signingCredentials = new(key, SecurityAlgorithms.HmacSha256);

		JwtSecurityToken jwt = new(
			Environment.MachineName,
			string.Empty,
			claims,
			DateTime.UtcNow,
			DateTime.UtcNow.AddHours(1),
			signingCredentials);

		var jwtInCompactSerializationFormat = new JwtSecurityTokenHandler().WriteToken(jwt);

		return TypedResults.Ok(jwtInCompactSerializationFormat);
	}
}
