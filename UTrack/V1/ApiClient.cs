using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UTrack.V1;

public class ApiClient(HttpClient httpClient, string xApiKey)
{
    public async Task<(IEnumerable<VesselCall>? vesselCalls, string error)> VesselCallListAsync(
       CancellationToken cancellationToken = default)
    {
        try
        {

            var request = new HttpRequestMessage(HttpMethod.Get, "vesselcall/list");
            request.Headers.Add("x-api-key", xApiKey);
            using var response = await httpClient.SendAsync(request, cancellationToken);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var items = await response.Content.ReadFromJsonAsync<IEnumerable<VesselCall>>(
                    _jsonOptions,
                    cancellationToken);
                return (items, string.Empty);
            }

            if (response.IsSuccessStatusCode)
            {
                return (null, string.Empty);
            }

            var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
            return (null, $"There is some problem occurred: {response.StatusCode}, Content: {errorContent}");

        }
        catch (OperationCanceledException)
        {
            return (null, "Request was canceled");
        }
        catch (Exception ex)
        {
            return (null, $"Unexpected error: {ex.Message}");
        }
    }

    public async Task<(CargoShipment? track, string error)> ShipmentSearchAsync(
        CargoSearchFilter filter,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await SendAndProcess(filter, "label", cancellationToken);

            if (string.IsNullOrEmpty(result.error) && result.track is null)
            {
                result = await SendAndProcess(filter, "bl_number", cancellationToken);
            }

            return result;
        }
        catch (OperationCanceledException)
        {
            return (null, "Request was canceled");
        }
        catch (Exception ex)
        {
            return (null, $"Unexpected error: {ex.Message}");
        }
    }


    public async Task<(IEnumerable<CargoShipment>? cargoShipments, string error)> NotifyShipmentGET(TgUser tgUser,  CancellationToken cancellationToken = default)
    {
        try
        {
            var xTguser = JsonSerializer.Serialize(tgUser, _jsonOptions);
            using var request = new HttpRequestMessage(HttpMethod.Get, $"notify/shipment");
            request.Headers.Add("x-api-key", xApiKey);
            request.Headers.Add("x-tguser", xTguser);
            using var response = await httpClient.SendAsync(request, cancellationToken);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var items = await response.Content.ReadFromJsonAsync<IEnumerable<CargoShipment>>(_jsonOptions, cancellationToken);
                return (items, string.Empty);
            }

            if (response.IsSuccessStatusCode)
            {
                return (null, string.Empty);
            }

            var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
            return (null, $"There is some problem occurred: {response.StatusCode}, Content: {errorContent}");

        }
        catch (OperationCanceledException)
        {
            return (null, "Request was canceled");
        }
        catch (Exception ex)
        {
            return (null, $"Unexpected error: {ex.Message}");
        }
    }


    #region Private helpers

    private async Task<(CargoShipment? track, string error)> SendAndProcess(CargoSearchFilter filter, string searchBy, CancellationToken cancellationToken)
    {
        using var request = await CreateRequest(filter, searchBy);
        using var response = await httpClient.SendAsync(request, cancellationToken);
        return await ProcessResult(response, cancellationToken);
    }

    private async Task<HttpRequestMessage> CreateRequest(CargoSearchFilter filter, string searchBy = "label") 
    {
        var queryParams = new Dictionary<string, string?>
        {
            { "search_by", searchBy },
            { "cargo_flow", filter.CargoFlow.ToSnakeCase() },
            { "is_container_unit", filter.IsContainerUnit.ToString().ToLowerInvariant() },
            { "is_empty_container_unit", filter.IsEmptyContainerUnit.ToString().ToLowerInvariant() }
        };

        var queryString = await new FormUrlEncodedContent(queryParams).ReadAsStringAsync();
        var body = new[] { filter.SearchString };

        var request = new HttpRequestMessage(HttpMethod.Post, $"shipment/search?{queryString}")
        {
            Content = JsonContent.Create(body)
        };
        request.Headers.Add("x-api-key", xApiKey);

        return request;
    }

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower) }
    };

    private static async Task<(CargoShipment? track, string error)> ProcessResult(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var items = await response.Content.ReadFromJsonAsync<IEnumerable<CargoShipment>>(
                _jsonOptions,
                cancellationToken);
            return (items?.FirstOrDefault(), string.Empty);
        }

        if (response.IsSuccessStatusCode)
        {
            return (null, string.Empty);
        }

        var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
        return (null, $"There is some problem occurred: {response.StatusCode}, Content: {errorContent}");
    }

    #endregion

}

public static class EnumExtensions
{
    public static string ToSnakeCase(this Enum value)
    {
        return JsonNamingPolicy.SnakeCaseLower.ConvertName(value.ToString());
    }
}