using drawIT.Database;
using drawIT.Models;
using drawIT.Services.Interfaces;
using HtmlAgilityPack;

namespace drawIT.Services
{
    public class AzureServiceScraper : IHostedService, IDisposable, IAzureServiceScraper
    {
        private readonly IAzureServiceDbContext _context;
        private static readonly HttpClient client = new HttpClient();
        private Timer? _timer;
        private readonly ILogger<AzureServiceScraper> _logger;

        public AzureServiceScraper(IAzureServiceDbContext context,
                                    ILogger<AzureServiceScraper> logger)
        {
            _context = context;
            _logger = logger;
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

        public async Task<List<AzureService>> StoreScrapedAzureServices()
        {
            var azureServices = new List<AzureService>();

            var response = await client.GetAsync("https://azure.microsoft.com/en-us/products/");
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

    }
}
