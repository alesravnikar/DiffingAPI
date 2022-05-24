using DiffingAPI.Models;
using DiffingAPI.Services;
using Xunit;

namespace Tests;

public class UnitTests
{
	private DiffingService DiffingService { get; } = new();

	[Fact]
	public void SameText()
	{
		string text1 = "AAAAAA==";
		string text2 = "AAAAAA==";

		DiffingService.AddLeft(1, text1);
		DiffingService.AddRight(1, text2);

		DiffingResult expected = new(DiffingResultType.Equals);
		DiffingResult? actual = DiffingService.GetDiff(1);

		Assert.Equal(expected, actual);
	}

	[Fact]
	public void DifferentText()
	{
		string text1 = "AAAAAA==";
		string text2 = "AQABAQ==";

		DiffingService.AddLeft(1, text1);
		DiffingService.AddRight(1, text2);

		DiffingResult expected = new(DiffingResultType.ContentDoNotMatch, new()
		{
			new(1, 1),
			new(3, 1),
			new(5, 1),
		});
		DiffingResult? actual = DiffingService.GetDiff(1);

		Assert.NotNull(actual);
		Assert.Equal(expected.Type, actual!.Type);
		Assert.Equal(expected.Diffs!.Count, actual!.Diffs!.Count);
		Assert.Equal(expected.Diffs[0], actual!.Diffs[0]);
		Assert.Equal(expected.Diffs[1], actual!.Diffs[1]);
		Assert.Equal(expected.Diffs[2], actual!.Diffs[2]);
	}

	[Fact]
	public void DifferentLengthText()
	{
		string text1 = "AAAAAA==";
		string text2 = "AAA=";

		DiffingService.AddLeft(1, text1);
		DiffingService.AddRight(1, text2);

		DiffingResult expected = new(DiffingResultType.SizeDoNotMatch);
		DiffingResult? actual = DiffingService.GetDiff(1);

		Assert.Equal(expected, actual);
	}
}
