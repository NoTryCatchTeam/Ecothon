using Ecothon.Web.Definitions.Contants;
using Ecothon.Web.Definitions.Strapi.Responses;
using Ecothon.Web.Helpers;

namespace Ecothon.Web.Services;

public class StrapiApiService : IStrapiApiService
{
    private readonly ILogger<StrapiApiService> _logger;
    private readonly HttpClient _httpClient;

    public StrapiApiService(IHttpClientFactory httpClientFactory, ILogger<StrapiApiService> logger)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient(HttpClientConstants.HttpClientNames.STRAPI_API);
    }

    // We now get only park with id = 1
    public async Task<ParkItemResponse> GetParkItemAsync()
    {
        var parkResponse = await _httpClient.GetAsync("parks/1");

        parkResponse.EnsureSuccessStatusCode();

        var park = JsonHelpers.Deserialize<BaseStrapiResponse<StrapiItemResponse<ParkItemResponse>>>(await parkResponse.Content.ReadAsStringAsync());

        return park.Data.Item;
    }
}
