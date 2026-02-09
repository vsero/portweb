using System.Net.Http.Json;
using System.Text.Json;
using Vsero.UTrcak;

namespace Vsero.UTrack;

internal static class HttpRequestExtensions
{
   
    public static HttpRequestMessage WithContent(this HttpRequestMessage request, object body)
    {
        request.Content = JsonContent.Create(body);
        return request;
    }



    public static HttpRequestMessage WithHeader(this HttpRequestMessage request, string headerKey, string headerValue)
    {
        request.Headers.Add(headerKey, headerValue);
        return request;
    }



    public static HttpRequestMessage WithHeaderXtguser(this HttpRequestMessage request, TgUser tgUser)
    {
        var xTguser = JsonSerializer.Serialize(tgUser, JsonOptions.SerializerOptions);
        request.Headers.Add("x-tguser", xTguser);
        return request;
    }


    public static HttpRequestMessage WithHeaderXapikey(this HttpRequestMessage request, string xApiKey)
    {
        request.Headers.Add("x-api-key", xApiKey);
        return request;
    }

}
