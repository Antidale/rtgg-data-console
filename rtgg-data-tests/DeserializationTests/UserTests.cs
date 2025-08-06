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
    "twitch_name": "testuser",
    "twitch_display_name": "TestUser",
    "twitch_channel": "https://www.twitch.tv/testuser",
    "can_moderate": true
}
""";

        var user = JsonSerializer.Deserialize<User>(jsonString);

        await Assert.That(user).IsNotNull();
        await Assert.That(user.CanModerate).IsTrue();
    }
}
