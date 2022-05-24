using System.Text.Json.Serialization;

namespace DiffingAPI.Models;

/// <summary>
/// Holds the starting index and length of the diff.
/// </summary>
/// <param name="Offset">Diff starting index.</param>
/// <param name="Length">Diff length.</param>
public record DiffItem
(
	[property: JsonPropertyName("offset")] int Offset,
	[property: JsonPropertyName("length")] int Length
);

