

namespace rtgg_data_console;

public class Queries
{
    /// <summary>
    /// Query for getting a race's id and status by the room name
    /// </summary>
    public const string GetRaceStatusByRoomNameQuery = @"select id, metadata->>Status' as Status
from races.race_detail
where room_name = @RoomName;";

    /// <summary>
    /// Inserts data in to races.race_detail. use roomName, raceType, raceHost, and metadata in params object
    /// </summary>
    public const string InsertRaceQuery = @"insert into races.race_detail(room_name, race_type, race_host, metadata)
values(@roomName, @raceType, @raceHost, @metadata)";

    /// <summary>
    /// Given an id from races.race_detail, updates the metadata of a race to have a Status of the value given to @status in the params object
    /// </summary>
    public const string UpdateRaceStatusQuery = @"update races.race_detail
    set metadata['Status'] = to_jsonb(@status ::text)
    where id = @id";

    /// <summary>
    /// Inserts into races.racers, based upon a passed in @racetimeId. Also include @raceId, @finishTime, @placement, and @metadata
    /// </summary>
    public const string InsertRaceEntrantQuery = @"insert into races.race_entrants(race_id, entrant_id, finish_time, placement, metadata)
    select id, @raceId, @finishTime, @placement, @metadata
    from races.racers where racetime_id = @racetimeId";

    /// <summary>
    /// Insert statement, returning the id, for a racer. use @racetimeName, @racetimeId, and @twitchName in params object
    /// </summary>
    public const string InsertRacerQuery = @"insert into races.racers (racetime_display_name, racetime_id, twitch_name)
    VALUES (@racetimeName, @racetimeId, @twitchName)
    returning id";

    public const string GetRacerQuery = @"";

}
