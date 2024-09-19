namespace Ecothon.Web.Definitions.Strapi.Responses;

public class HabitantStatus
{
    public string Description { get; set; }

    public int Status { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }
}
