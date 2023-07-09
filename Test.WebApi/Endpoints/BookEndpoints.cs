using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Test.Application.DTOs;
using Test.Application.Mappers;
using Test.Domain;
using Test.Infrastructure.DbContexts;

namespace Test.WebApi.Endpoints;

public static class BookEndpoints
{
	public static IEndpointRouteBuilder MapBookEndpoints(this IEndpointRouteBuilder builder)
	{
		var group = builder.MapGroup("/books")
			.WithTags(nameof(Book) + 's');

		group.MapPost("/", CreateBook)
			.WithName(nameof(CreateBook));

		group.MapGet("/", GetBooks)
			.WithName(nameof(GetBooks));

		group.MapGet("/{id}", GetBook)
			.WithName(nameof(GetBook));

		group.WithOpenApi();

		return builder;
	}

	private static async Task<Created<BookResponse>> CreateBook(AppDbContext db, BookMapper bookMapper, CreateBookRequest createBookRequest)
	{
		Book book = bookMapper.MapToBook(createBookRequest);

		db.Authors.Attach(book.Author);
		
		await db.Books.AddAsync(book);
		await db.SaveChangesAsync();

		BookResponse bookResponse = bookMapper.MapToBookResponse(book);

		return TypedResults.Created($"books/{bookResponse.Id}", bookResponse);
	}

	private static async Task<Results<Ok<BookResponse[]>, NoContent>> GetBooks(AppDbContext db, BookMapper bookMapper)
	{
		Book[] books = await db.Books.ToArrayAsync();

		BookResponse[] bookDtos = books.Select(bookMapper.MapToBookResponse).ToArray();

		if (books.Any())
		{
			return TypedResults.Ok(bookDtos);
		}

		return TypedResults.NoContent();
	}

	private static async Task<Results<Ok<BookResponse>, NotFound>> GetBook(AppDbContext db, BookMapper bookMapper, Guid id)
	{
		Book? book = await db.Books.FindAsync(id);

		if (book is null)
		{
			return TypedResults.NotFound();
		}

		BookResponse bookResponse = bookMapper.MapToBookResponse(book);

		return TypedResults.Ok(bookResponse);
	}
}
