using Ecothon.Web.Definitions.Contants;
using Ecothon.Web.Definitions.Strapi.Responses;
using Ecothon.Web.Helpers;
using System.Net.Http;
using System.Net.Http.Json;

namespace Ecothon.Web.Services;

public class StrapiApiServiceMock : IStrapiApiService
{
    private HttpClient _httpClient;
    public StrapiApiServiceMock(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(HttpClientConstants.HttpClientNames.LOCAL);
    }

    public async Task<IEnumerable<HabitantItemResponse>> GetHabitantsAsync()
    {
        var habitants = await _httpClient.GetFromJsonAsync<HabitantItemResponse[]>("jsons/habitants.json");
        return habitants;
    }

    public async Task<IEnumerable<ParkItemResponse>> GetParksAsync()
    {
        var parks = await _httpClient.GetFromJsonAsync<ParkItemResponse[]>("jsons/parks.json");
        return parks;
    }
}