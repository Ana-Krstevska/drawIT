using drawIT.Database;
using drawIT.Models;
using drawIT.Services.Interfaces;
using HtmlAgilityPack;
using MongoDB.Driver;

namespace drawIT.Services
{
    public class AzureServiceScraper : IHostedService, IDisposable, IAzureServiceScraper
    {
        private readonly IDbContext _context;
        private readonly HttpClient _client;
        private Timer? _timer;
        private readonly ILogger<AzureServiceScraper> _logger;

        public AzureServiceScraper(IDbContext context,
                                    ILogger<AzureServiceScraper> logger,
                                    IHttpClientFactory clientFactory)
        {
            _context = context;
            _logger = logger;
            _client = clientFactory.CreateClient();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Azure service scraper is starting.");

            _timer = new Timer(async _ => await StoreScrapedAzureServices(), null, TimeSpan.Zero, TimeSpan.FromDays(7));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Azure service scraper is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public async Task<List<AzureService>> GetCloudServicesAsync()
        {
            var azureServices = new List<AzureService>();

            var response = await _client.GetAsync("https://azure.microsoft.com/en-us/products/");
            var pageContents = await response.Content.ReadAsStringAsync();

            var pageDocument = new HtmlDocument();
            pageDocument.LoadHtml(pageContents);

            var headers = pageDocument.DocumentNode.Descendants("h3")
                .Where(h => h.Attributes.Contains("class") && h.Attributes["class"].Value.Contains("h5"))
                .Select(h => h.InnerText);

            foreach (var header in headers)
            {
                var sanitizedHeader = header.Replace("ᴾᴿᴱⱽᴵᴱᵂ", "").Trim();  
                azureServices.Add(new AzureService { Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(), Name = sanitizedHeader });
            }

            return azureServices;
        }

        public async Task<List<AzureService>> StoreScrapedAzureServices()
        {
            var azureServices = await GetCloudServicesAsync();
            List<string> duplicatesNames = new List<string>();

            try
            {
                foreach (var azureService in azureServices)
                {
                    var filter = Builders<AzureService>.Filter.Eq(s => s.Name, azureService.Name);
                    var existingService = await _context.AzureServices.Find(filter).FirstOrDefaultAsync();

                    if (existingService == null)
                    {
                        await _context.AzureServices.InsertOneAsync(azureService);
                    }
                    else
                    {
                        duplicatesNames.Add(azureService.Name);
                        _logger.LogInformation($"Duplicate found: Service with name {azureService.Name} already exists.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error writing down records");
            }

            _logger.LogInformation($"Total duplicates found: {duplicatesNames.Count}. Names: {string.Join(", ", duplicatesNames)}");

            return azureServices;
        }

    }
}
