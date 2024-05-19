namespace BookswormAPI.Models;

public class FavouriteBook
{
    public int BookId { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Contributor { get; set; }
    public decimal Price { get; set; }
    public int Rate { get; set; }
}