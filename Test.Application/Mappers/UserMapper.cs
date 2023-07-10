using Riok.Mapperly.Abstractions;
using Test.Application.DTOs;
using Test.Domain;

namespace Test.Application.Mappers;

[Mapper]
public partial class UserMapper
{
	[MapperIgnoreTarget(nameof(User.Id))]
	[MapProperty(nameof(CreateUserRequest.Password), nameof(User.PasswordHash))]
	public partial User MapToUser(CreateUserRequest createUserRequest);

	[MapperIgnoreSource(nameof(User.Login))]
	[MapperIgnoreSource(nameof(User.PasswordHash))]
	public partial UserResponse MapToUserResponse(User user);
}

[Mapper]
public static partial class UserMapperExtensions
{
	[MapperIgnoreSource(nameof(User.Login))]
	[MapperIgnoreSource(nameof(User.PasswordHash))]
	public static partial IQueryable<UserResponse> MapToUserResponse(this IQueryable<User> users);
}
