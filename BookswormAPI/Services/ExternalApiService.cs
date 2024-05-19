using BookswormAPI.Configuration;
using Microsoft.Extensions.Options;

namespace BookswormAPI.Services;

public class ExternalApiService
{
    private readonly HttpClient _httpClient;
    private readonly IOptions<NewYorkTimesApi> _settings;

    public ExternalApiService(IHttpClientFactory httpClientFactory, IOptions<NewYorkTimesApi> settings)
    {
        _httpClient = httpClientFactory.CreateClient();
        _settings = settings;
    }
    
    public async Task<string> GetNewYorkTimesData()
    {
        var url = _settings.Value.Url;
        var key = _settings.Value.Key;

        _httpClient.BaseAddress = new Uri($"{url}");

        var response = await _httpClient.GetStringAsync($"{url}{key}");

        return response;
    }
}