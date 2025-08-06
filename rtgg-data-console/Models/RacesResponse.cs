namespace rtgg_data_console.Models;

public record class RacesResponse
{
    public int Count { get; set; }
    public int NumPages { get; set; }
    public List<Race> Races { get; set; } = [];
}
