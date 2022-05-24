using System.Text.Json.Serialization;

namespace DiffingAPI.Models;

/// <summary>
/// Class for getting data from diffing api put requests.
/// </summary>
/// <param name="Data">Base64 encoded binary data</param>
public record DiffingData([property: JsonPropertyName("data")] string? Data);