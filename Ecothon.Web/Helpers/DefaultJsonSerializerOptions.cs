using System.Text.Json;

namespace Ecothon.Web.Helpers;

public static class DefaultJsonSerializerOptions
{
    private static JsonSerializerOptions _instance;

    public static JsonSerializerOptions Instance =>
        _instance ??= new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
}
