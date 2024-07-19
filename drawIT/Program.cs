using drawIT.API.Services;
using drawIT.API.Services.Interfaces;
using drawIT.Database;
using drawIT.Services;
using drawIT.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IApplicationBuilder, ApplicationBuilder>();  
builder.Services.AddScoped<IAzureServiceScraper, AzureServiceScraper>();
builder.Services.AddScoped<IAWSServiceScraper, AWSServiceScraper>();
builder.Services.AddScoped<IDrawingRequestService, DrawingRequestService>();
builder.Services.AddScoped<IDatabaseService, DatabaseService>();
builder.Services.AddSingleton<IDbContext, DbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
