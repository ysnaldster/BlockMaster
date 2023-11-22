using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BlockMaster.Domain.Request.Identity;
using BlockMaster.Tests.Util;
using Newtonsoft.Json;

namespace BlockMaster.Tests.Extensions;

public static class AuthenticationExtension
{
    public static async Task<string> GenerateAuthenticationToken(this HttpClient httpClient)
    {
        var tokenRequest = new TokenGenerationRequest
        {
            UserId = Guid.NewGuid().ToString(),
            Email = "user@mail.com",
            CustomClaims = new Dictionary<string, object>
            {
                { "admin", "true" }
            }
        };
        var request = new HttpRequestMessage(HttpMethod.Post, $"block-master/v1/identity/token");
        var requestSerialize = JsonConvert.SerializeObject(tokenRequest);
        request.Content = new StringContent(requestSerialize, Encoding.UTF8, "application/json");
        var response = await httpClient.SendAsync(request);
        var token = TokenUtils.ExtractToken(await response.Content.ReadAsStringAsync());
        
        return token;
    }
}