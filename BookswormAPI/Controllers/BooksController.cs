using BookswormAPI.Data;
using BookswormAPI.Data.Models;
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
        var userId = User.Claims.Select(x => x.Value).FirstOrDefault();
        var user = await _userManager.FindByEmailAsync(userId);

        if (user == null)
        {
            return BadRequest();
        }
        
        var favouriteBooks = await _dbContext.UserFavouriteBooks.Include(x => x.Book)
            .Where(x => x.UserId == user.Id)
            .Select(x => new FavouriteBook()
            {
                BookId = x.BookId
            })
            .ToListAsync();

        var result = await _dbContext.Books.Select(x => new BookResponse()
        {
            Title = x.Title,
            Author = x.Author,
            Price = x.Price,
            Contributor = x.contributor,
            BookImage = x.BookImage,
            IsFavourite = favouriteBooks.Exists(f => f.BookId == x.Id),
        }).ToListAsync();
        
        return Ok(result);
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
                BookImage = x.Book.BookImage
            }).ToListAsync();
        
        return Ok(favouriteBooks);
    }

    [HttpPost]
    public async Task<IActionResult> FindFavouriteBook([FromBody] SearchBook searchBook)
    {
        var userId = User.Claims.Select(x => x.Value).FirstOrDefault();
        var user = await _userManager.FindByEmailAsync(userId);
        if (user == null)
        {
            return BadRequest();
        }

        var favouriteBooks = await _dbContext.UserFavouriteBooks.Include(x => x.Book)
            .Where(x => x.UserId == user.Id && (x.Book.Title.Contains(searchBook.UserQuery) ||
                                                x.Book.Author.Contains(searchBook.UserQuery)))
            .Select(x => new FavouriteBook()
            {
                BookId = x.BookId,
                Title = x.Book.Title,
                Author = x.Book.Author,
                Contributor = x.Book.contributor,
                Price = x.Book.Price,
                Rate = x.Rate,
                BookImage = x.Book.BookImage
            })
            .ToListAsync();

        return Ok(favouriteBooks);
    }

    [HttpPost]
    public async Task<IActionResult> AddBookToFavourite([FromBody] AddBookToFavourite addBookToFavourite)
    {
        var userId = User.Claims.Select(x => x.Value).FirstOrDefault();
        var user = await _userManager.FindByEmailAsync(userId);
        if (user == null)
        {
            return BadRequest();
        }
        var book = await _dbContext.Books.FirstOrDefaultAsync(b => b.Id == addBookToFavourite.BookId);

        if (book != null)
        {
            return Ok("Book Already in your favourites");
        }
        
        var userFavouriteBook = new UserFavouriteBook()
        {
            UserId = user.Id, BookId = addBookToFavourite.BookId, Rate = 0
        };

        _dbContext.UserFavouriteBooks.Add(userFavouriteBook);
        await _dbContext.SaveChangesAsync();

        return Ok("Book saved to favourites");
    }

    [HttpPost]
    public async Task<IActionResult> RemoveBookFromFavourite([FromBody] RemoveBookFromFavourite removeBookFromFavourite)
    {
        var userId = User.Claims.Select(x => x.Value).FirstOrDefault();
        var user = await _userManager.FindByEmailAsync(userId);
        if (user == null)
        {
            return BadRequest();
        }

        var favouriteBook = await _dbContext.UserFavouriteBooks.FirstOrDefaultAsync(x =>
            x.UserId == user.Id && x.BookId == removeBookFromFavourite.BookId);
        if (favouriteBook == null)
        {
            return NotFound();
        }

        _dbContext.UserFavouriteBooks.Remove(favouriteBook);
        await _dbContext.SaveChangesAsync();

        return Ok();
    }
    
}