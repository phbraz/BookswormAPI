namespace BookswormAPI.Models;

public class SaveBookToFavourite
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int Rate { get; set; }
    public decimal Price { get; set; }
    public string userEmail { get; set; }
    public bool IsFavourite { get; set; }
}