using BookswormAPI.Data.Models;
using BookswormAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookswormAPI.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    { }
    
    public DbSet<Book> Books { get; set; }
    public DbSet<UserFavouriteBook> UserFavouriteBooks { get; set; }
    
}