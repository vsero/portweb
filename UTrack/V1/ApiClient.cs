using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;


namespace UTrack.V1;

public class ApiClient(HttpClient httpClient)
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower) }
    };

    public async Task<(ShipmentTrack? track, string error)> ShipmentSearchAsync(string searchString)
    {
        
        var queryParams = new Dictionary<string, string?>
        {
            { "search_by", "label" },
            { "is_empty_container_unit", "false" },
            { "cargo_flow", "inbound" }
        };

        var queryString = await new FormUrlEncodedContent(queryParams).ReadAsStringAsync();

        var body = new[] { searchString };
        var request = new HttpRequestMessage(HttpMethod.Post, $"shipment/search?{queryString}")
        {
            Content = JsonContent.Create(body)
        };

        request.Headers.Add("x-api-key", "QPUGvnCqzJSyisAMwCescyrz");
        var response = await httpClient.SendAsync(request);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var items = await response.Content.ReadFromJsonAsync<IEnumerable<ShipmentTrack>>(_jsonOptions);
            return (items?.FirstOrDefault(), string.Empty);
        }
        else if (response.IsSuccessStatusCode)
        {
            return (null, string.Empty);
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            return (null, $"There is some problem occurred: {response.StatusCode}, Content: {errorContent}");
        }
    }
}
