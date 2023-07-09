using Test.Domain;

namespace Test.Application.DTOs;

public record CreateBookRequest(string Title, string Description, Genre Genre, Guid AuthorId);