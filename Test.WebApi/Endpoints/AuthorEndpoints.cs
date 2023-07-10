using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Test.Application.DTOs;
using Test.Application.Mappers;
using Test.Domain;
using Test.Infrastructure.DbContexts;

namespace Test.WebApi.Endpoints;

public static class AuthorEndpoints
{
	public static IEndpointRouteBuilder MapAuthorEndpoints(this IEndpointRouteBuilder builder)
	{
		var group = builder.MapGroup("/authors")
			.WithTags(nameof(Author) + 's')
			.WithOpenApi();

		group.MapPost("/", CreateAuthor)
			.WithName(nameof(CreateAuthor));

		group.MapGet("/", GetAuthors)
			.WithName(nameof(GetAuthors));

		group.MapDelete("/{id}", DeleteAuthor)
			.WithName(nameof(DeleteAuthor));

		return builder;
	}

	private static async Task<Created<AuthorResponse>> CreateAuthor(AppDbContext db, AuthorMapper authorMapper, CreateAuthorRequest createAuthorRequest)
	{
		Author author = authorMapper.MapToAuthor(createAuthorRequest);

		await db.Authors.AddAsync(author);
		await db.SaveChangesAsync();

		AuthorResponse authorResponse = authorMapper.MapToAuthorResponse(author);

		return TypedResults.Created($"authors/{authorResponse.Id}", authorResponse);
	}

	private static async Task<Results<Ok<AuthorResponse[]>, NoContent>> GetAuthors(AppDbContext db)
	{
		AuthorResponse[] authorDtos = await db.Authors.MapToAuthorResponse().ToArrayAsync();

		if (authorDtos.Any())
		{
			return TypedResults.Ok(authorDtos);
		}

		return TypedResults.NoContent();
	}

	private static async Task<Results<NoContent, NotFound>> DeleteAuthor(AppDbContext db, Guid id)
	{
		Author? author = await db.Authors.FindAsync(id);

		if (author is null)
		{
			return TypedResults.NotFound();
		}

		db.Authors.Remove(author);
		await db.SaveChangesAsync();

		return TypedResults.NoContent();
	}
}