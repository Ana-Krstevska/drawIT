using drawIT.API.Services.Interfaces;
using drawIT.Database;
using drawIT.Models;
using drawIT.Services.Interfaces;
using MongoDB.Driver;


namespace drawIT.API.Services
{
    public class DrawingRequestService : IDrawingRequestService
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly IAzureServiceScraper _azureServiceScraper;
        private readonly IAzureServiceDbContext _context;
        private readonly ILogger<DrawingRequestService> _logger;

        public DrawingRequestService(IAzureServiceDbContext context,
            IAzureServiceScraper azureServiceScraper, ILogger<DrawingRequestService> logger)
        {
            _context = context;
            _logger = logger;
            _azureServiceScraper = azureServiceScraper;
        }

        public async Task<bool> RegisterDrawingRequestAsync()
        {
            return false;
        }

        public async Task<DrawingRequest> GetDrawingRequestAsync(string id)
        {
            return null;
        }

        public async Task<List<AzureService>> GetCloudServicesAsync()
        {
            var azureServices = await _azureServiceScraper.StoreScrapedAzureServices();
            try
            {
                foreach (var azureService in azureServices)
                {
                    await _context.AzureServices.InsertOneAsync(azureService);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error writing down records");
            }

            return azureServices;
        }

    }
}
