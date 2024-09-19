namespace Ecothon.Web.Definitions.Strapi.Responses;

public class BaseStrapiResponse<TResult>
{
    public TResult Data { get; set; }

    // public StrapiMetaResponse Meta { get; set; }

    // public StrapiErrorResponse Error { get; set; }
}
