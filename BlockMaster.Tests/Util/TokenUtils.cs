using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json.Linq;

namespace BlockMaster.Tests.Util;

[ExcludeFromCodeCoverage]
public static class TokenUtils
{
    private static string _token;
    public static void SetToken(string token)
    {
        _token = token;
    }
    
    public static string GetToken()
    {
        return _token;
    }
    
    public static string ExtractToken(string jsonToken)
    {
        var jsonObject = JObject.Parse(jsonToken);
        var token = jsonObject["token"]?.ToString();

        return token;
    }
}