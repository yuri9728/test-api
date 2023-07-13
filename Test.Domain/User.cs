using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Test.Domain;

[Index(nameof(Login))]
public class User
{
	public Guid Id { get; set; }

	[Required]
	[MaxLength(50)]
	public string Login { get; set; } = string.Empty;

	[Required]
	[MaxLength(50)]
	public string PasswordHash { get; set; } = string.Empty;
}
