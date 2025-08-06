using System;
using System.Text.Json;
using rtgg_data_console.Models;

namespace rtgg_data_tests.DeserializationTests;

public class EntrantTests
{
    [Test]
    public async Task FinishedDeserialzesCorrectly()
    {
        var jsonString = """
{
    "user": {
        "id": "c0lumb0",
        "full_name": "the_prelate#5035",
        "name": "judthe_prelateejoe00",
        "discriminator": "5035",
        "url": "/user/c0lumb0/the_prelate",
        "avatar": null,
        "pronouns": "he/him",
        "flair": "",
        "twitch_name": "the_prelate",
        "twitch_display_name": "the_prelate",
        "twitch_channel": "https://www.twitch.tv/the_prelate",
        "can_moderate": false
    },
    "team": null,
    "status": {
        "value": "done",
        "verbose_value": "Finished",
        "help_text": "Finished the race."
    },
    "finish_time": "P0DT01H10M21.370993S",
    "finished_at": "2025-08-02T02:10:55.968Z",
    "place": 1,
    "place_ordinal": "1st",
    "score": 2603,
    "score_change": 151,
    "comment": "O frabjous day!",
    "has_comment": false,
    "stream_live": true,
    "stream_override": false,
    "actions": []
}
""";
        var sut = JsonSerializer.Deserialize<Entrant>(jsonString, JsonSerializerOptions.Web);
        await Assert.That(sut).IsNotNull()
            .And.HasMember(x => x.Place).EqualTo(1)
            .And.HasMember(x => x.Comment).EqualTo("O frabjous day!")
            .And.HasMember(x => x.Status.Value).EqualTo("done")
            .And.HasMember(x => x.Score).EqualTo(2603)
            .And.HasMember(x => x.ScoreChange).EqualTo(151);
    }

    [Test]
    public async Task ForfeitDeserialzesCorrectly()
    {
        var jsonString = """
{
    "user": {
        "id": "abcdefg123456",
        "full_name": "someperson#1234",
        "name": "someperson",
        "discriminator": "1234",
        "url": "/user/abcdefg123456/someperson",
        "avatar": null,
        "pronouns": "they/them",
        "flair": "moderator",
        "twitch_name": "someperson",
        "twitch_display_name": "SomePerson",
        "twitch_channel": "https://www.twitch.tv/someperson",
        "can_moderate": true
    },
    "team": null,
    "status": {
        "value": "dnf",
        "verbose_value": "DNF",
        "help_text": "Did not finish the race."
    },
    "finish_time": null,
    "finished_at": null,
    "place": null,
    "place_ordinal": null,
    "score": 1886,
    "score_change": -25,
    "comment": null,
    "has_comment": false,
    "stream_live": true,
    "stream_override": false,
    "actions": []
}
""";
        var sut = JsonSerializer.Deserialize<Entrant>(jsonString, JsonSerializerOptions.Web);

        await Assert.That(sut).IsNotNull()
            .And.HasMember(x => x.Status.Value).EqualTo("dnf")
            .And.HasMember(x => x.ScoreChange).EqualTo(-25)
            .And.HasMember(x => x.FinishTime).EqualTo(null)
            .And.HasMember(x => x.Score).EqualTo(1886)
            .And.HasMember(x => x.Comment).EqualTo(null)
            .And.HasMember(x => x.Place).EqualTo(null);
    }
}
