using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BookswormAPI.Data.Models;

public class UserFavouriteBook
{
    [Key]
    public int Id { get; set; }
    
    public string UserId { get; set; }
    public int BookId { get; set; }
    
    [Range(0, 5)]
    public int Rate { get; set; }
    
    [ForeignKey("UserId")]
    public virtual IdentityUser User { get; set; }
    
    [ForeignKey("BookId")] 
    public virtual Book Book { get; set; }
}