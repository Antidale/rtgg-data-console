using System;
using System.Text.Json;
using rtgg_data_console.Models;

namespace rtgg_data_tests.DeserializationTests;

public class GoalTests
{
    [Test]
    public async Task GoalDeserializesCorrectly()
    {
        var jsonText = """
{
    "name": "Beat Zeromus",
    "custom": false
}
""";
        var sut = JsonSerializer.Deserialize<Goal>(jsonText, JsonSerializerOptions.Web);
        await Assert.That(sut).IsNotNull()
            .And.HasMember(x => x.Name).EqualTo("Beat Zeromus")
            .And.HasMember(x => x.Custom).EqualTo(false);
    }

    [Test]
    public async Task CustomGoalDeserializesCorrectly()
    {
        var jsonText = """
{
    "name": "Beat Zeromus (custom)",
    "custom": true
}
""";
        var sut = JsonSerializer.Deserialize<Goal>(jsonText, JsonSerializerOptions.Web);
        await Assert.That(sut).IsNotNull()
            .And.HasMember(x => x.Name).EqualTo("Beat Zeromus (custom)")
            .And.HasMember(x => x.Custom).EqualTo(true);
    }
}
