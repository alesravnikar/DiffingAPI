using System.Text.Json.Serialization;

namespace DiffingAPI.Models;

/// <summary>
/// Result of the diffing operation.
/// </summary>
/// <param name="Type">Diffing result type.</param>
/// <param name="Diffs">List of diffs, null unless Type is ContentDoNotMatch.</param>
public record DiffingResult
(
	[property: JsonPropertyName("diffResultType")] DiffingResultType Type,
	[property: JsonPropertyName("diffs")] List<DiffItem>? Diffs = null
);

