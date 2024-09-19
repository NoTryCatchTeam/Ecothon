namespace Ecothon.Web.Definitions.Strapi.Responses;

public class StrapiPaginationResponse
{
    public int Page { get; set; }

    public int PageSize { get; set; }

    public int PageCount { get; set; }

    public int Total { get; set; }
}
