using System.Text;
using Newtonsoft.Json;

namespace Finder.Domain.Helpers;

public static class JsonBase64Helper
{
    public static string Encode<T>(T json)
    {
        var jsonString = JsonConvert.SerializeObject(json);
        var byteArray = Encoding.UTF8.GetBytes(jsonString);
        var base64String = Convert.ToBase64String(byteArray);

        return base64String;
    }

    public static T? Decode<T>(string base64)
    {
        var decodedByteArray = Convert.FromBase64String(base64);
        var decodedJsonString = Encoding.UTF8.GetString(decodedByteArray);
        var decodedJson = JsonConvert.DeserializeObject<T>(decodedJsonString);

        return decodedJson;
    }
}