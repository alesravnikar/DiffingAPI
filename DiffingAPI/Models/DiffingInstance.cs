namespace DiffingAPI.Models;

/// <summary>
/// Class for storing left and right values for diff comparison.
/// </summary>
/// <param name="Left">Left value.</param>
/// <param name="Right">Right value.</param>
public record DiffingInstance(string? Left, string? Right);

