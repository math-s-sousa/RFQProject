using CustomRFQ.Databases;
using CustomRFQ.Models;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<Context>();

// Add services to the container.
var app = builder.Build();

app.UseCors(builder => builder
.AllowAnyOrigin()
.AllowAnyMethod()
.AllowAnyHeader());

// Configure the HTTP request pipeline.
app.MapGet("/", () => "Hello World!");

app.MapGet("/rfq/{id}", (string id, Context context) =>
{
	try
	{
		var eventObject = context._conn.GetEvent(id);

		var slInstance = context._conn._dbs.Where(a => a.DB == eventObject.DB).FirstOrDefault();

        var getQuotation = slInstance.SLApi.Get($"PurchaseQuotations({eventObject.DocEntry})").Result;

		var quotation = JsonSerializer.Deserialize<ServiceLayer.MarketingDocument.Value>(getQuotation.success);

        return Results.Ok(quotation);
	}
    catch (Exception ex)
	{
		return Results.Problem(ex.Message);
	}
});

app.MapPost("/rfq/{id}", (string id, ServiceLayer.MarketingDocument.Value body, Context context) =>
{
	try
	{
        var eventObject = context._conn.GetEvent(id);

        var slInstance = context._conn._dbs.Where(a => a.DB == eventObject.DB).FirstOrDefault();

        var pLoad = new { body.DocumentLines };

		var patchQuotation = slInstance.SLApi.Patch($"PurchaseQuotations({eventObject.DocEntry})", JsonSerializer.Serialize(pLoad)).Result;

		if (patchQuotation.success)
			return Results.Ok(new { Status = 200, detail = "updated"});

		else
			return Results.Problem(patchQuotation.failed.error.message.value);
	}
	catch (Exception ex)
	{
		return Results.Problem(ex.Message);
	}
});

app.MapPost("/migrate", () => {
	try
	{
		//new Migrate(_sL);

		return Results.NoContent();
	}
	catch (Exception ex)
	{
		return Results.Problem(ex.Message);
	}
});

app.Run();