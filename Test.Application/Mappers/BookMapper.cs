using Riok.Mapperly.Abstractions;
using Test.Application.DTOs;
using Test.Domain;

namespace Test.Application.Mappers;

[Mapper]
public partial class BookMapper
{
	public Book MapToBook(CreateBookRequest createBookRequest)
	{
		Book book = CreateBookRequestToBook(createBookRequest);

		book.Author = new() { Id = createBookRequest.AuthorId };

		return book;
	}

	[MapperIgnoreSource(nameof(Book.Author))]
	public partial BookResponse MapToBookResponse(Book book);

	[MapperIgnoreTarget(nameof(Book.Id))]
	[MapperIgnoreSource(nameof(CreateBookRequest.AuthorId)), MapperIgnoreTarget(nameof(Book.Author))]
	private partial Book CreateBookRequestToBook(CreateBookRequest createBookRequest);
}
