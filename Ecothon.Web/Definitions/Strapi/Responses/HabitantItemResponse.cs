namespace Ecothon.Web.Definitions.Strapi.Responses;

public class HabitantItemResponse
{
    public string NameRu { get; set; }

    public string NameLat { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public string Population { get; set; }

    public string Spreading { get; set; }

    // public object Habitat { get; set; }
    // public object Limiting { get; set; }
    // public object Security { get; set; }
    // public object Measures { get; set; }
    // public Gallery Gallery { get; set; }

    public BaseStrapiResponse<StrapiItemResponse<StrapiGeneralMedia>> Photo { get; set; }

    public BaseStrapiResponse<StrapiItemResponse<HabitantStatus>> Status { get; set; }
}
