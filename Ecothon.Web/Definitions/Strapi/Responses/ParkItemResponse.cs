using System.Text.Json.Serialization;

namespace Ecothon.Web.Definitions.Strapi.Responses;

public class ParkItemResponse
{
    [JsonPropertyName("NameRu")]
    public string Name { get; set; }

    public string Description { get; set; }

    public string Location { get; set; }

    public Polygon Polygon { get; set; }
}
