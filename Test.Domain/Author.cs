using System.ComponentModel.DataAnnotations;

namespace Test.Domain;

public class Author
{
	public Guid Id { get; set; }

	[Required]
	[MaxLength(150)]
	public string Name { get; set; } = string.Empty;

	public ICollection<Book> Books { get; set; } = null!;
}