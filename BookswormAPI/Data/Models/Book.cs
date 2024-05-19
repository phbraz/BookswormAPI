using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BookswormAPI.Data.Models;

public class Book
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Title { get; set; }
    
    [Required]
    public string Author { get; set; }
    
    public decimal Price { get; set; }
    
    public string contributor { get; set; }

    public string? BookImage { get; set; }
}