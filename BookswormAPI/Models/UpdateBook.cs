namespace BookswormAPI.Models;

public class UpdateBook
{
    public int BookId { get; set; }
    public decimal Price { get; set; }
    public int Rate { get; set; }
}