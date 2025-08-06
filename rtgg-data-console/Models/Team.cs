namespace rtgg_data_console.Models;

public record class Team
{
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public bool Formal { get; set; }
}
