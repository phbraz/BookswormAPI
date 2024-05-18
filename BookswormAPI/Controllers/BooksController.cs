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

    [HttpPost]
    public async Task<IActionResult> FavouriteBooksByUser([FromBody] LoginModel loginModel)
    {
        var user = await _userManager.FindByEmailAsync(loginModel.Email);
        if (user == null)
        {
            return BadRequest();
        }
        
        var books = await _dbContext.Books.Where(b => b.UserId == user.Id.ToString()).ToListAsync();
        return Ok(books);
    }

    [HttpPost]
    public async Task<IActionResult> SaveBookToFavourite([FromBody] SaveBookToFavourite saveBookToFavourite)
    {
        var user = await _userManager.FindByEmailAsync(saveBookToFavourite.userEmail);
        
        if (user == null)
        {
            return BadRequest();
        }
        
        var existingBook = await _dbContext.Books.FirstOrDefaultAsync(b => b.Title == saveBookToFavourite.Title && b.Author == saveBookToFavourite.Author && b.UserId == user.Id.ToString());
        
        if (existingBook != null)
        {
            existingBook.Rate = saveBookToFavourite.Rate;
            existingBook.Price = saveBookToFavourite.Price;
            existingBook.IsFavourite = saveBookToFavourite.IsFavourite;
            _dbContext.Books.Update(existingBook);
        }
        else
        {
            var book = new Book()
            {
                Title = saveBookToFavourite.Title,
                Author = saveBookToFavourite.Author,
                Rate = saveBookToFavourite.Rate,
                Price = saveBookToFavourite.Price,
                IsFavourite = saveBookToFavourite.IsFavourite,
                UserId = user.Id.ToString()
            };
            _dbContext.Books.Add(book);
        }

        await _dbContext.SaveChangesAsync();
        
        return Ok(saveBookToFavourite);
    }
}