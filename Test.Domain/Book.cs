using System.ComponentModel.DataAnnotations;

namespace Test.Domain;

public class Book
{
	public Guid Id { get; set; }

	[Required]
	[MaxLength(100)]
	public string Title { get; set; } = string.Empty;

	[MaxLength(1000)]
	public string Description { get; set; } = string.Empty;

	public Genre Genre { get; set; } = Genre.None;

	public Author Author { get; set; } = null!;
}