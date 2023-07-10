using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Test.Application.DTOs;
using Test.Application.Mappers;
using Test.Domain;
using Test.Infrastructure.DbContexts;

namespace Test.WebApi.Endpoints;

public static class UserEndpoints
{
	public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder builder)
	{
		var group = builder.MapGroup("/users")
			.WithTags(nameof(User) + 's')
			.WithOpenApi();

		group.MapPost("/", CreateUser)
			.WithName(nameof(CreateUser));

		group.MapGet("/", GetUsers)
			.WithName(nameof(GetUsers));

		group.MapDelete("/{id}", DeleteUser)
			.WithName(nameof(DeleteUser));

		return builder;
	}

	private static async Task<Created<UserResponse>> CreateUser(AppDbContext db, UserMapper userMapper, CreateUserRequest createUserRequest)
	{
		User user = userMapper.MapToUser(createUserRequest);

		await db.Users.AddAsync(user);
		await db.SaveChangesAsync();

		UserResponse userResponse = userMapper.MapToUserResponse(user);

		return TypedResults.Created($"books/{userResponse.Id}", userResponse);
	}

	private static async Task<Results<Ok<UserResponse[]>, NoContent>> GetUsers(AppDbContext db, UserMapper bookMapper)
	{
		User[] users = await db.Users.ToArrayAsync();

		UserResponse[] bookDtos = users.Select(bookMapper.MapToUserResponse).ToArray();

		if (users.Any())
		{
			return TypedResults.Ok(bookDtos);
		}

		return TypedResults.NoContent();
	}

	private static async Task<Results<NoContent, NotFound>> DeleteUser(AppDbContext db, Guid id)
	{
		User? user = await db.Users.FindAsync(id);

		if (user is null)
		{
			return TypedResults.NotFound();
		}

		db.Users.Remove(user);
		await db.SaveChangesAsync();

		return TypedResults.NoContent();
	}
}
