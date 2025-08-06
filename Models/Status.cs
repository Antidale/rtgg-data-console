using System.Text.Json.Serialization;

namespace rtgg_data_console.Models;

public record class Status
{
    public string Value { get; set; } = string.Empty;
    [JsonPropertyName("verbose_value")]
    public string VerboseValue { get; set; } = string.Empty;
    [JsonPropertyName("help_text")]
    public string HelpText { get; set; } = string.Empty;
}
