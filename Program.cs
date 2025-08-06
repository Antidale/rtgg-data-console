// See https://aka.ms/new-console-template for more information
using rtgg_data_console;
using rtgg_data_console.Models;
using rtgg_data_console.Providers;

var httpClient = new HttpClient
{
    BaseAddress = new Uri("https://racetime.gg/ff4fe/")
};

var connectionProvier = new ConnectionProvider();
var rtggApi = new RtggApi();

int totalRaces = 0;
int pageSize = 100;

try
{
    //figure out how many calls it requires to pull at 100 races/call
    totalRaces = await rtggApi.GetTotalRacesCountAsync();
    var requiredPageCount = totalRaces / pageSize + (totalRaces % pageSize > 0 ? 1 : 0);


    var raceList = new List<Race>(capacity: totalRaces);

    //then make them all
    for (var currentPage = 1; currentPage <= requiredPageCount; currentPage++)
    {
        var stuff = await rtggApi.GetRaceDataAsync(currentPage, pageSize);
        raceList.AddRange(stuff);
    }

    var connectionProvider = new ConnectionProvider();
    var repository = new Repository(connectionProvider);

    // pull out all the User parts into a distinct list and insert them into races.racers
    var userData = raceList.SelectMany(x => x.Entrants.Select(x => x.User)).Distinct();
    foreach (var user in userData)
    {
        await repository.InsertAsync<int>(Queries.InsertRacerQuery, new { });
    }



    // looping through all the races
    foreach (var race in raceList)
    {
        // check to see if the race exists
        var existingRace = await repository.GetAsync<(int id, string status)>(Queries.GetRaceStatusByRoomNameQuery, new { RoomName = race.Name.Split("/").Last() });

        //  if so: update the status to the correct status (but don't overwrite if I've already added a custom status)
        if (existingRace.id > 0)
        {
            if (string.IsNullOrEmpty(existingRace.status))
            {
                await repository.UpdateAsync(Queries.UpdateRaceStatusQuery, new { });
            }
        }
        //  if not: insert the race
        else
        {
            var id = await repository.InsertAsync<int>(Queries.InsertRaceQuery, new { });
        }



        // insert into races.race_entrants the details for each racer in the race


    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}


