using System.Text.Json;

namespace Ecothon.Web.Helpers;

public static class JsonHelpers
{
    public static TResult Deserialize<TResult>(string json) =>
        JsonSerializer.Deserialize<TResult>(json, DefaultJsonSerializerOptions.Instance);
}
