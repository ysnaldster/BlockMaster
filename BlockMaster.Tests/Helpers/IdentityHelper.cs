using Newtonsoft.Json.Linq;

namespace BlockMaster.Tests.Helpers;

public static class IdentityHelper
{
    public static string ExtractToken(string jsonToken)
    {
        var jsonObject = JObject.Parse(jsonToken);
        var token = jsonObject["token"]?.ToString();

        return token;
    }
}