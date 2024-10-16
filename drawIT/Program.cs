using drawIT.API.Services;
using drawIT.API.Services.Interfaces;
using drawIT.Database;
using drawIT.Services;
using drawIT.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder
            .WithOrigins("http://localhost:4200") // Frontend origin
            .AllowAnyMethod()
            .AllowAnyHeader());
});

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
builder.Services.AddScoped<ILlamaService, LlamaService>();
builder.Services.AddScoped<ISuggestionService, SuggestionService>();

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
app.UseCors("AllowSpecificOrigin");
app.UseAuthorization();
app.MapControllers();
app.Run();
