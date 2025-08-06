

namespace rtgg_data_console.Entities;

public class RacerEntity
{
#pragma warning disable IDE1006 // Naming Styles - ignoring for pure database models, these names reflect what is in the database

    public int id { get; init; }
    public string racetime_display_name { get; init; } = string.Empty;
    public string racetime_id { get; init; } = string.Empty;
    public string twitch_name { get; init; } = string.Empty;

#pragma warning restore IDE1006 // Naming Styles
}
