using HackerNewsApiConsumer.Interfaces;
using HackerNewsApiConsumer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
builder.Services.AddTransient<IStoryService, StoryService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseCors(options => options
	.WithOrigins("https://hackernewsapiconsumerapi.azure-api.net", "http://localhost:4200")
	.AllowAnyHeader()
	.AllowAnyMethod());
app.UseHttpsRedirection();

app.MapControllers();

app.Run();
