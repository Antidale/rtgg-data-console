using System;

namespace rtgg_data_console.Entities;

public class EntrantEntity
{
#pragma warning disable IDE1006 // Naming Styles - ignoring for pure database models, these names reflect what is in the database

    public int race_id { get; init; }
    public int entrant_id { get; init; }
    public string finish_time { get; init; } = string.Empty;
    public int? placement { get; init; }
    public Dictionary<string, string> metadata { get; init; } = [];

#pragma warning restore IDE1006 // Naming Styles
}
