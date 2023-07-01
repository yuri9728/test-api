using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Test.Domain;
using Test.Infrastructure.DbContexts;

namespace Test.WebApi.Endpoints;

public static class BookEndpoints
{
	public static IEndpointRouteBuilder MapBookEndpoints(this IEndpointRouteBuilder builder)
	{
		var group = builder.MapGroup("/books")
			.WithTags(nameof(Book) + 's');

		group.MapGet("/", GetBooks)
			.WithName(nameof(GetBooks));

		group.MapGet("/{id}", GetBook)
			.WithName(nameof(GetBook));

		group.WithOpenApi();

		return builder;
	}

	private static async Task<Results<Ok<Book[]>, NoContent>> GetBooks(AppDbContext db)
	{
		Book[] books = await db.Books.ToArrayAsync();

		if (books.Any())
		{
			return TypedResults.Ok(books);
		}

		return TypedResults.NoContent();
	}

	private static async Task<Results<Ok<Book>, NotFound>> GetBook(AppDbContext db, Guid id)
	{
		Book? book = await db.Books.FindAsync(id);

		if (book is null)
		{
			return TypedResults.NotFound();
		}

		return TypedResults.Ok(book);
	}
}
