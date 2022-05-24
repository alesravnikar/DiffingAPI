using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using DiffingAPI.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace IntegrationTests;

public class IntegrationTests
{
	private JsonSerializerOptions Options { get; }

	public IntegrationTests()
	{
		Options = new(JsonSerializerDefaults.Web)
		{
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
		};

		Options.Converters.Add(new JsonStringEnumConverter());
	}

	[Fact]
	public async Task SampleTest()
	{
		WebApplicationFactory<Program> app = new();

		HttpClient client = app.CreateClient();

		// 1
		HttpResponseMessage response = await client.GetAsync("/v1/diff/1");

		Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

		// 2
		DiffingData data = new("AAAAAA==");
		StringContent content = new(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

		response = await client.PutAsync("/v1/diff/1/left", content);

		Assert.Equal(HttpStatusCode.Created, response.StatusCode);

		// 3
		response = await client.GetAsync("/v1/diff/1");

		Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

		// 4
		data = new("AAAAAA==");
		content = new(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

		response = await client.PutAsync("/v1/diff/1/right", content);

		Assert.Equal(HttpStatusCode.Created, response.StatusCode);

		// 5
		response = await client.GetAsync("/v1/diff/1");

		Assert.Equal(HttpStatusCode.OK, response.StatusCode);

		string result = await response.Content.ReadAsStringAsync();
		DiffingResult? diffingResult = JsonSerializer.Deserialize<DiffingResult>(result, Options);

		Assert.NotNull(diffingResult);
		Assert.Equal(DiffingResultType.Equals, diffingResult!.Type);

		// 6
		data = new("AQABAQ==");
		content = new(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

		response = await client.PutAsync("/v1/diff/1/right", content);

		Assert.Equal(HttpStatusCode.Created, response.StatusCode);

		// 7
		response = await client.GetAsync("/v1/diff/1");

		Assert.Equal(HttpStatusCode.OK, response.StatusCode);

		result = await response.Content.ReadAsStringAsync();
		diffingResult = JsonSerializer.Deserialize<DiffingResult>(result, Options);

		Assert.NotNull(diffingResult);
		Assert.Equal(DiffingResultType.ContentDoNotMatch, diffingResult!.Type);
		Assert.Equal(3, diffingResult!.Diffs!.Count);
		Assert.Equal(new DiffItem(1, 1), diffingResult!.Diffs![0]);
		Assert.Equal(new DiffItem(3, 1), diffingResult!.Diffs![1]);
		Assert.Equal(new DiffItem(5, 1), diffingResult!.Diffs![2]);

		// 8
		data = new("AAA=");
		content = new(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

		response = await client.PutAsync("/v1/diff/1/left", content);

		Assert.Equal(HttpStatusCode.Created, response.StatusCode);

		// 9
		response = await client.GetAsync("/v1/diff/1");

		Assert.Equal(HttpStatusCode.OK, response.StatusCode);

		result = await response.Content.ReadAsStringAsync();
		diffingResult = JsonSerializer.Deserialize<DiffingResult>(result, Options);

		Assert.NotNull(diffingResult);
		Assert.Equal(DiffingResultType.SizeDoNotMatch, diffingResult!.Type);

		// 10
		data = new(null);
		content = new(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

		response = await client.PutAsync("/v1/diff/1/left", content);

		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
	}
}