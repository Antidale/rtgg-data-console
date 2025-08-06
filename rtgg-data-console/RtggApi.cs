using System.Net.Http.Json;
using rtgg_data_console.Models;

namespace rtgg_data_console;

public class RtggApi
{
    private static readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("https://racetime.gg/ff4fe/races/data")
    };

    public RacesResponse? MostRecentResponse { get; set; }

    public async Task<int> GetTotalRacesCountAsync()
    {
        var response = await QueryRacesEndpoint(page: 1, perPage: 1);

        if (response is null) return 0;

        try
        {
            var responseData = await response.Content.ReadFromJsonAsync<RacesResponse>();
            if (responseData is null)
            {
                Console.WriteLine("failed to deserialize response data");
                return 0;
            }

            MostRecentResponse = responseData;
            return responseData.Count;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return 0;
        }
    }

    public async Task<List<Race>> GetRaceDataAsync(int page, int perPage)
    {
        Console.WriteLine($"requesting data for page {page}");
        var queryResponse = await QueryRacesEndpoint(page, perPage);

        if (queryResponse is null) return [];

        try
        {
            var racesData = await queryResponse.Content.ReadFromJsonAsync<RacesResponse>();
            MostRecentResponse = racesData;
            return racesData?.Races ?? [];
        }
        catch (Exception ex)
        {
            Console.WriteLine($"failed to deserialize response data: {ex.Message}");
            return [];
        }
    }

    private static async Task<HttpResponseMessage?> QueryRacesEndpoint(int page, int perPage)
    {
        try
        {
            var response = await _httpClient.GetAsync(QueryString(page: page, perPage: perPage));
            response.EnsureSuccessStatusCode();
            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    static string QueryString(int page, int perPage) => $"?page={page}&per_page={perPage}&show_entrants=1";
}
