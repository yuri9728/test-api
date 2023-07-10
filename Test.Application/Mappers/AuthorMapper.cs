using Riok.Mapperly.Abstractions;
using Test.Application.DTOs;
using Test.Domain;

namespace Test.Application.Mappers;


[Mapper]
public partial class AuthorMapper
{
	[MapperIgnoreTarget(nameof(Author.Id))]
	[MapperIgnoreTarget(nameof(Author.Books))]
	public partial Author MapToAuthor(CreateAuthorRequest createAuthorRequest);

	[MapperIgnoreSource(nameof(Author.Books))]
	public partial AuthorResponse MapToAuthorResponse(Author author);
}