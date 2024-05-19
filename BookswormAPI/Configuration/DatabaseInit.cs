using BookswormAPI.Data;
using BookswormAPI.Services;

namespace BookswormAPI.Configuration;

public class DatabaseInit
{
    private readonly ExternalApiService _externalApiService;
    private readonly DataSeed _dataSeed;

    public DatabaseInit(ExternalApiService externalApiService, DataSeed dataSeed)
    {
        _dataSeed = dataSeed;
        _externalApiService = externalApiService;
    }
    
    //Seeding the API data to the DB 
    public async Task DataSeedingAsync()
    {
        var bookDataExists = await _dataSeed.IfExists();
        if (!bookDataExists)
        {
            var jsonData = await _externalApiService.GetNewYorkTimesData();
            await _dataSeed.SeedData(jsonData);
        }
    }

}