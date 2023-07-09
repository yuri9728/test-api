using Test.Domain;

namespace Test.Application.DTOs;

public record BookResponse(Guid Id, string Title, string Description, Genre Genre);
