using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BookswormAPI.Data.Models;

public class Book
{
    [Key]
    public int Id { get; set; }

    public string UserId { get; set; }

    [Required]
    public string Title { get; set; }
    
    [Required]
    public string Author { get; set; }

    [Range(0, 5)]
    public int Rate { get; set; }

    public decimal Price { get; set; }

    public bool IsFavourite { get; set; }

    [ForeignKey("UserId")]
    public virtual IdentityUser User { get; set; }
    
}