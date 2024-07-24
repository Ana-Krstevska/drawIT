using drawIT.API.Services;
using drawIT.API.Services.Interfaces;
using drawIT.Database;
using drawIT.Services;
using drawIT.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

builder.Services.AddScoped<IApplicationBuilder, ApplicationBuilder>();
builder.Services.AddSingleton<IDbContext, DbContext>();
builder.Services.AddSingleton<IAzureServiceScraper, AzureServiceScraper>();
builder.Services.AddSingleton<IAWSServiceScraper, AWSServiceScraper>();
builder.Services.AddScoped<IDrawingRequestService, DrawingRequestService>();
builder.Services.AddScoped<IDatabaseService, DatabaseService>();

builder.Services.AddHostedService<AWSServiceScraper>();
builder.Services.AddHostedService<AzureServiceScraper>();

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
