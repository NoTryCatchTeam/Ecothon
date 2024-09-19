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

    public async Task<IEnumerable<ParkItemResponse>> GetParksAsync()
    {
        var parkResponse = await _httpClient.GetAsync("parks");

        parkResponse.EnsureSuccessStatusCode();

        var park = JsonHelpers.Deserialize<BaseStrapiResponse<IEnumerable<StrapiItemResponse<ParkItemResponse>>>>(await parkResponse.Content.ReadAsStringAsync());

        return park.Data.Select(d => d.Item).ToArray();
    }

    public async Task<IEnumerable<HabitantItemResponse>> GetHabitantsAsync()
    {
        var habitantsResponse = await _httpClient.GetAsync("rb-objects?populate[gallery]=*&populate[photo]=*&populate[status]=*&populate[family]=*");

        habitantsResponse.EnsureSuccessStatusCode();

        var habitants = JsonHelpers.Deserialize<BaseStrapiResponse<IEnumerable<StrapiItemResponse<HabitantItemResponse>>>>(await habitantsResponse.Content.ReadAsStringAsync());

        return habitants.Data.Select(x => x.Item).ToArray();
    }
}
