// See https://aka.ms/new-console-template for more information
using Dapper;
using rtgg_data_console;
using rtgg_data_console.Models;
using rtgg_data_console.Providers;
using rtgg_data_console.SqlMappers;

var httpClient = new HttpClient
{
    BaseAddress = new Uri("https://racetime.gg/ff4fe/")
};

var connectionProvier = new ConnectionProvider();
var rtggApi = new RtggApi();

int totalRaces = 0;
int pageSize = 100;

SqlMapper.AddTypeHandler(new StringListHandler());
SqlMapper.AddTypeHandler(new JsonStringDictionaryHandler());


//figure out how many calls it requires to pull at 100 races/call
totalRaces = await rtggApi.GetTotalRacesCountAsync();
var requiredPageCount = totalRaces / pageSize + (totalRaces % pageSize > 0 ? 1 : 0);

var raceList = new List<Race>(capacity: 100);

//then make them all
for (var currentPage = 1; currentPage <= requiredPageCount; currentPage++)
{
    var stuff = await rtggApi.GetRaceDataAsync(currentPage, pageSize);
    raceList.AddRange(stuff);
}

var connectionProvider = new ConnectionProvider();
var repository = new Repository(connectionProvider);

// pull out all the User parts into a distinct list and insert them into races.racers
var userData = raceList.SelectMany(x => x.Entrants.Select(x => x.User)).Distinct().Select(x => new { racetimeName = x.Name, racetimeId = x.Id, twitchName = x.TwitchDisplayName });
foreach (var user in userData)
{
    await repository.InsertAsync<int>(Queries.InsertRacerQuery, new { user.racetimeName, user.racetimeId, user.twitchName });
}

// looping through all the races
foreach (var race in raceList)
{
    var roomName = race.Name.Split("/").Last();
    // check to see if the race exists
    var (id, status) = await repository.GetAsync<(int id, string status)>(
        Queries.GetRaceStatusByRoomNameQuery,
        new { roomName }
    );

    if (id > 0)
    {
        if (string.IsNullOrEmpty(status) && race.Status.Value is not null)
        {
            await repository.UpdateAsync(
                Queries.UpdateRaceStatusQuery,
                new { id, status = race.Status.Value }
            );
        }
    }
    else
    {
        var metaData = new Dictionary<string, string>
        {
            ["Goal"] = race.Goal.Name,
            ["Description"] = race.Info
        };

        if (!string.IsNullOrWhiteSpace(race.Status.Value))
            metaData.Add("Status", race.Status.Value);

        id = await repository.InsertAsync<int>(
            Queries.InsertRaceQuery, new { roomName, metaData }
        );
    }

    // // insert into races.race_entrants the details for each racer in the race
    foreach (var entrant in race.Entrants)
    {
        var metaData = new Dictionary<string, string>();
        if (entrant.Score.HasValue)
            metaData.Add("score", entrant.Score!.Value.ToString());

        if (entrant.ScoreChange.HasValue)
            metaData.Add("scoreChange", entrant.ScoreChange!.Value.ToString());

        if (!string.IsNullOrWhiteSpace(entrant.Comment))
            metaData.Add("comment", entrant.Comment);

        if (entrant.Status.Value != "done")
            metaData.Add("status", entrant.Status.Value);

        await repository.InsertAsync(
                    Queries.InsertRaceEntrantQuery,
                    new { raceId = id, finishTime = entrant.FinishTime, placement = entrant.Place, metaData, racetimeId = entrant.User.Id }
                );
    }
}
