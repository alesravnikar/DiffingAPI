using DiffingAPI.Models;

namespace DiffingAPI.Services;

public class DiffingService
{
	// in-memory cache
	// Dictionary would be terrible for a real app, but works for demonstration purposes
	private Dictionary<int, DiffingInstance> DiffingCache { get; } = new();

	public void AddLeft(int id, string value)
	{
		if (DiffingCache.ContainsKey(id))
		{
			DiffingCache[id] = DiffingCache[id] with { Left = value };
		}
		else
		{
			DiffingCache.Add(id, new(value, null));
		}
	}

	public void AddRight(int id, string value)
	{
		if (DiffingCache.ContainsKey(id))
		{
			DiffingCache[id] = DiffingCache[id] with { Right = value };
		}
		else
		{
			DiffingCache.Add(id, new(null, value));
		}
	}

	/// <summary>
	/// Gets the diff result for the specified id.
	/// </summary>
	/// <param name="id">Diff id.</param>
	/// <returns>DiffingResult if left and right are initialized, null otherwise.</returns>
	public DiffingResult? GetDiff(int id)
	{
		if (!DiffingCache.TryGetValue(id, out DiffingInstance? diff)) return null;
		if (diff.Left is null) return null;
		if (diff.Right is null) return null;

		char[] leftChars = diff.Left.ToCharArray();
		char[] rightChars = diff.Right.ToCharArray();

		if (leftChars.Length != rightChars.Length) return new(DiffingResultType.SizeDoNotMatch);

		List<DiffItem> diffs = new();

		int i = 0;
		while (i < leftChars.Length)
		{
			if (leftChars[i] != rightChars[i])
			{
				int offset = i;
				int length = 1;
				i++;

				while (leftChars[i] != rightChars[i])
				{
					length++;
					i++;
				}

				diffs.Add(new(offset, length));
			}

			i++;
		}

		return diffs switch
		{
			{ Count: 0 } => new(DiffingResultType.Equals),
			_ => new(DiffingResultType.ContentDoNotMatch, diffs)
		};
	}
}

