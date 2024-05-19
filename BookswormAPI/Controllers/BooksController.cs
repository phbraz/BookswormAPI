using BookswormAPI.Data;
using BookswormAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookswormAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]/[action]")]
public class BooksController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ApplicationDbContext _dbContext;

    public BooksController(UserManager<IdentityUser> userManager, ApplicationDbContext dbContext, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _dbContext = dbContext;
        _signInManager = signInManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetBooks()
    {
        var books = await _dbContext.Books.ToListAsync();
        return Ok(books);
    }

    [HttpPost]
    public async Task<IActionResult> FindBook([FromBody] SearchBook searchBook)
    {
        var books = await _dbContext.Books.Where(b => b.Title.Contains(searchBook.UserQuery) || b.Author.Contains(searchBook.UserQuery)).ToListAsync();
        return Ok(books);
    }

    [HttpGet]
    public async Task<IActionResult> GetFavouriteBooksByUser()
    {
        var userId = User.Claims.Select(x => x.Value).FirstOrDefault();
        var user = await _userManager.FindByEmailAsync(userId);
        if (user == null)
        {
            return BadRequest();
        }

        var favouriteBooks = await _dbContext.UserFavouriteBooks
            .Include(x => x.Book).Where(x => x.UserId == user.Id)
            .Select(x => new FavouriteBook()
            {
                BookId = x.BookId,
                Title = x.Book.Title,
                Author = x.Book.Author,
                Contributor = x.Book.contributor,
                Price = x.Book.Price,
                Rate = x.Rate,
            }).ToListAsync();
        
        return Ok(favouriteBooks);
    }
    
}