using System.Text.Json;
using rtgg_data_console.Models;

namespace rtgg_data_tests.DeserializationTests;

public class UserTests
{
    [Test]
    public async Task UserDeserializesCorrectly()
    {
        var jsonString = """
{
    "id": "zM65aWXa4bB1y8q0",
    "full_name": "TestUser#7297",
    "name": "testuser",
    "discriminator": "7297",
    "url": "/user/zM65aWXa4bB1y8q0/testuser",
    "avatar": "https://test.org/media/zM65aWXa4bB1y8q0.jpg",
    "pronouns": "she/they",
    "flair": "",
    "twitch_name": "testusertwitch",
    "twitch_display_name": "TestUserTwitch",
    "twitch_channel": "https://www.twitch.tv/testuser",
    "can_moderate": true
}
""";
        //use the Web setting because the main project is always using `ReadFromJsonAsync` which uses that option
        var user = JsonSerializer.Deserialize<User>(jsonString, JsonSerializerOptions.Web);

        await Assert
            .That(user).IsNotNull()
            .And.HasMember(x => x.Name).EqualTo("testuser")
            .And.HasMember(x => x.Pronouns).EqualTo("she/they")
            .And.HasMember(x => x.TwitchName).EqualTo("testusertwitch")
            .And.HasMember(x => x.TwitchDisplayName).EqualTo("TestUserTwitch");
    }
}
