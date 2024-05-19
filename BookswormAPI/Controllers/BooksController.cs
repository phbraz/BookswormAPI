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
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> SaveBookToFavourite([FromBody] SaveBookToFavourite saveBookToFavourite)
    {
        var user = await _userManager.FindByEmailAsync(saveBookToFavourite.userEmail);
        
        if (user == null)
        {
            return BadRequest();
        }
        
        return Ok();
    }
}