using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Vsero.UTrack;

namespace Vsero.UTrcak.V1;

public class ApiClient(HttpClient httpClient, string xApiKey)
{


    public async Task<(IEnumerable<VesselCall>? vesselCalls, string error)> VesselCallListAsync(CancellationToken ct = default)
    {
        var request = NewApiRequest(HttpMethod.Get, "vesselcall/list");
        (var items, string _er) = await ExecuteAndReadContentAsync<IEnumerable<VesselCall>>(request, ct);
        return (items, _er);
    }



    public async Task<(IEnumerable<CargoShipment>? cargoShipments, string error)> NotifyShipmentGetAsync(TgUser tgUser, CancellationToken ct = default)
    {
        using var request = NewApiRequest(HttpMethod.Get, "notify/shipment")
            .WithHeaderXtguser(tgUser);

        (var items, string _er) = await ExecuteAndReadContentAsync<IEnumerable<CargoShipment>>(request, ct);
        return (items, _er);
    }



    public async Task<(bool result, string error)> NotifyShipmentPostAsync(TgUser tgUser, CargoShipment shipment, CancellationToken ct = default)
    {
        using var request = NewApiRequest(HttpMethod.Post, $"notify/shipment/{shipment.Uuid.ToString()}")
            .WithHeaderXtguser(tgUser);

        (var response, string _er) = await ExecuteRequestAsync(request, ct);
        return (response?.IsSuccessStatusCode ?? false, _er);
    }



    public async Task<(bool result, string error)> NotifyShipmentDeleteAsync(TgUser tgUser, CargoShipment shipment, CancellationToken ct = default)
    {
        using var request = NewApiRequest(HttpMethod.Delete, $"notify/shipment/{shipment.Uuid.ToString()}")
            .WithHeaderXtguser(tgUser);

        (var response, string _er) = await ExecuteRequestAsync(request, ct);
        return (response?.IsSuccessStatusCode ?? false, _er);
    }



    public async Task<(CargoShipment? track, string error)> ShipmentSearchAsync(CargoSearchFilter filter, CancellationToken ct = default)
    {
        var request = await ShipmentSearchRequest(filter, "label");

        (var shipment, string _er) = await ExecuteAndReadContentAsync<IEnumerable<CargoShipment>>(request, ct);
        if (shipment is null)
        {
            request = await ShipmentSearchRequest(filter, "bl_number");
            (shipment, _er) = await ExecuteAndReadContentAsync<IEnumerable<CargoShipment>>(request, ct);
        }
        return (shipment?.FirstOrDefault(), _er);
    }



    private async Task<HttpRequestMessage> ShipmentSearchRequest(CargoSearchFilter filter, string searchBy = "label")
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
        var request = NewApiRequest(HttpMethod.Post, $"shipment/search?{queryString}")
            .WithContent(body);
        
        return request;
    }



    private async Task<(T? t, string error)> ExecuteAndReadContentAsync<T>(HttpRequestMessage request, CancellationToken ct) where T : class
    {
        (HttpResponseMessage? response, string _er) = await ExecuteRequestAsync(request, ct);
        if (response is not null && response.StatusCode == HttpStatusCode.OK)
        {
            using (response)
            {
                (T? result, _er) = await ReadJSONContentAsync<T>(response, ct);
                return (result, _er);
            }
        }
        return (null, _er);
    }



    private async Task<(HttpResponseMessage? result, string error)> ExecuteRequestAsync(HttpRequestMessage request, CancellationToken ct)
    {
        try
        {

            var response = await httpClient.SendAsync(request, ct);
            if (response.IsSuccessStatusCode)
            {
                return (response, string.Empty);
            }

            var statusCode = response.StatusCode;
            var errorContent = await response.Content.ReadAsStringAsync(ct);
            response.Dispose();
            return (null, $"Problem: {statusCode}, Content: {errorContent}");

        }
        catch (OperationCanceledException)
        {
            return (null, "Request timeout");
        }
        catch (Exception ex)
        {
            return (null, $"Unexpected error: {ex.Message}");
        }
    }



    private async Task<(T? t, string error)> ReadJSONContentAsync<T>(HttpResponseMessage response, CancellationToken ct) where T : class
    {
        try
        {

            if (response.Content == null)
                return (null, "Response content is empty");

            var data = await response.Content.ReadFromJsonAsync<T>(JsonOptions.SerializerOptions, ct);
            return (data, string.Empty);

        }
        catch (JsonException ex)
        {
            return (null, $"JSON Deserialization error: {ex.Message}");
        }
        catch (Exception ex)
        {
            return (null, $"Unexpected error during reading: {ex.Message}");
        }

    }



    private HttpRequestMessage NewApiRequest(HttpMethod method, string uri) =>
        new HttpRequestMessage(method, uri).WithHeaderXapikey(xApiKey);

}