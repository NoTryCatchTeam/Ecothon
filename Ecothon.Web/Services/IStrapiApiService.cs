using Ecothon.Web.Definitions.Strapi.Responses;

namespace Ecothon.Web.Services;

public interface IStrapiApiService
{
    Task<ParkItemResponse> GetParkItemAsync();
}
