using DiffingAPI.Models;
using DiffingAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

JsonSerializerOptions options = new(JsonSerializerDefaults.Web)
{
	DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
};

options.Converters.Add(new JsonStringEnumConverter());

builder.Services.AddSingleton<DiffingService>();

WebApplication app = builder.Build();


app.MapPut("/v1/diff/{id}/left", (DiffingService diffingService, int id, [FromBody] DiffingData diffingData) =>
{
	if (diffingData.Data is null) return Results.BadRequest();

	diffingService.AddLeft(id, diffingData.Data);

	return Results.Created("", null);
});

app.MapPut("/v1/diff/{id}/right", (DiffingService diffingService, int id, [FromBody] DiffingData diffingData) =>
{
	if (diffingData.Data is null) return Results.BadRequest();

	diffingService.AddRight(id, diffingData.Data);

	return Results.Created("", null);
});

app.MapGet("/v1/diff/{id}", (DiffingService diffingService, int id) =>
{
	DiffingResult? result = diffingService.GetDiff(id);

	if (result is null) return Results.NotFound();

	return Results.Json(result, options);
});

app.Run();

public partial class Program
{
	// Expose the Program class for use with WebApplicationFactory<T>
}