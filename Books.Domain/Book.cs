namespace Books.Domain;

public class Book
{
	public Guid Id { get; set; }
	public string Title { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public Genre Genre { get; set; } = Genre.None;
	
	public Author Author { get; set; }
}