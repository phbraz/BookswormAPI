namespace BookswormAPI.Models;

public class BookResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public int Rate { get; set; }
    public decimal Price { get; set; }
    public string Contributor { get; set; }
    public string BookImage { get; set; }
    public bool IsFavourite { get; set; }
}