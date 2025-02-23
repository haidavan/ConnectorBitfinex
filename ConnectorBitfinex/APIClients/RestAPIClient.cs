using System.Globalization;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ConnectorBitfinex.APIClients;

public class RestAPIClient
{
    private HttpClient _httpClient;
    private const string _baseUrl = "https://api-pub.bitfinex.com/v2/";

    public RestAPIClient()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(_baseUrl);
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

    }
    public async Task<string> GetTradeAsync(string pair, int maxCount)
    {
        var uriBuilder = new UriBuilder(_httpClient.BaseAddress + $"trades/{pair}/hist");
        var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);

        query["limit"] = maxCount.ToString();

        uriBuilder.Query = query.ToString();

        HttpResponseMessage response = await _httpClient.GetAsync(uriBuilder.Uri.ToString());
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
    public async Task<string> GetCandleSeriesAsync(string pair, string timeFrame, DateTimeOffset? from,
        DateTimeOffset? to = null, long? count = 0)
    {
        var uriBuilder = new UriBuilder(_httpClient.BaseAddress + $"candles/tickerdata:{timeFrame}:{pair}/hist");
        var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
        Console.WriteLine(uriBuilder.Uri.ToString());

        query["start"] = from.ToString();
        query["end"] = to.ToString();
        query["limit"] = count.ToString();

        uriBuilder.Query = query.ToString();

        HttpResponseMessage response = await _httpClient.GetAsync(uriBuilder.Uri.ToString());
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
    public async Task<string> GetTickerAsync(string pair)
    {
        var uriBuilder = new UriBuilder(_httpClient.BaseAddress + $"ticker/{pair}");

        HttpResponseMessage response = await _httpClient.GetAsync(uriBuilder.Uri.ToString());
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
