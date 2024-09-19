using System.Text.Json.Serialization;

namespace Ecothon.Web.Definitions.Strapi.Responses;

public class StrapiItemResponse<TItem>
{
    public int Id { get; set; }

    [JsonPropertyName("attributes")]
    public TItem Item { get; set; }
}
