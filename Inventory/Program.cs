using Inventory.Repositories;
using Inventory.Settings;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Libmongocrypt;
using System.Text.Json;
using System.Net.Mime;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
BsonSerializer.RegisterSerializer(new GuidSerializer(MongoDB.Bson.BsonType.String));            //Pretypovani Guid a DateTimeOffset na string pro snadnejsi praci v mongodb
BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(MongoDB.Bson.BsonType.String));
var mongoDbSettings = builder.Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
//Dependency injection
//tady se rika JAKOU instanci vytvorit    _
builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
	return new MongoClient(mongoDbSettings.ConnectionString);
});
//tady se rika jaky typ instance vytvorit             _
builder.Services.AddSingleton<IItemsRepository, MongoDbItemsRepository>();
builder.Services.AddControllers(options =>
{
	options.SuppressAsyncSuffixInActionNames = false;
});
builder.Services.AddHealthChecks()
	.AddMongoDb(
		mongoDbSettings.ConnectionString,
		name: "mongodb",
		timeout: TimeSpan.FromSeconds(3),
		tags: new[] { "ready" });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseRouting();

app.UseEndpoints(endpoints =>
{
	endpoints.MapControllers();

	endpoints.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
	{
		Predicate = (check) => check.Tags.Contains("ready"),
		ResponseWriter = async (context, report) =>
		{
			var result = JsonSerializer.Serialize(
					new
					{
						status = report.Status.ToString(),
						checks = report.Entries.Select(entry => new
						{
							name = entry.Key,
							checks = entry.Value.Status.ToString(),
							exception = entry.Value.Exception != null ? entry.Value.Exception.ToString() : "none",
							duration = entry.Value.Duration.ToString()
						})
					}
				);
			context.Response.ContentType = MediaTypeNames.Application.Json;
			await context.Response.WriteAsync( result );
		}
	});
	endpoints.MapHealthChecks("/health/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
	{
		Predicate = (_) => false
	});
});


app.MapHealthChecks("/health");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
