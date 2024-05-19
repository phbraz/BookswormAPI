using BookswormAPI.Data.Models;
using BookswormAPI.DTOS;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using JsonConverter = System.Text.Json.Serialization.JsonConverter;

namespace BookswormAPI.Data;

public class DataSeed
{
    private ApplicationDbContext _dbContext;

    public DataSeed(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> IfExists()
    {
        return await _dbContext.Books.AnyAsync();
    }

    public async Task SeedData(string jsonData)
    {
        var booksData = JsonConvert.DeserializeObject<BooksRootResponseDto>(jsonData);
        var booksToSeed = booksData.results.books.Select(x => new Book()
        {
            Title = x.Title,
            Author = x.Author,
            Price = x.Price,
            contributor = x.contributor,
            BookImage = x.book_image
        });
        await _dbContext.Books.AddRangeAsync(booksToSeed);
        await _dbContext.SaveChangesAsync();
    }
}