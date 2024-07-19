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
        private readonly IAWSServiceScraper _awsServiceScraper;

        public DrawingRequestService(IAzureServiceDbContext context,
            IAzureServiceScraper azureServiceScraper, 
            IAWSServiceScraper awsServiceScraper,
            ILogger<DrawingRequestService> logger)
        {
            _context = context;
            _logger = logger;
            _awsServiceScraper = awsServiceScraper;
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
                    var filter = Builders<AzureService>.Filter.Eq(s => s.Name, azureService.Name);
                    var existingService = await _context.AzureServices.Find(filter).FirstOrDefaultAsync();

                    if (existingService == null)
                    {
                        await _context.AzureServices.InsertOneAsync(azureService);
                    }
                    else
                    {
                        _logger.LogInformation($"Service with name {azureService.Name} already exists.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error writing down records");
            }

            return azureServices;
        }

        public async Task<List<AWSService>> GetAWSCloudServicesAsync()
        {
            var awsServices = await _awsServiceScraper.StoreScrapedAWSServices(); // make sure this includes the category now    
            _logger.LogInformation($"Retrieved {awsServices.Count} services from scraper.");

            try
            {
                foreach (var awsService in awsServices)
                {
                    _logger.LogInformation($"Processing service: {awsService.Name}, category: {awsService.Category}");

                    var filterByName = Builders<AWSService>.Filter.Eq(s => s.Name, awsService.Name);
                    var existingServices = await _context.AWSServices.Find(filterByName).ToListAsync();

                    if (!existingServices.Any())
                    {
                        await _context.AWSServices.InsertOneAsync(awsService);
                    }
                    else
                    {
                        var isServiceWithSameCategoryExists = existingServices.Any(s => s.Category == awsService.Category);
                        if (!isServiceWithSameCategoryExists)
                        {
                            await _context.AWSServices.InsertOneAsync(awsService);
                        }
                        else
                        {
                            _logger.LogInformation($"Service with name {awsService.Name} and category {awsService.Category} already exists.");
                        }
                    }
                }

                var countAfterProcessing = await _context.AWSServices.CountDocumentsAsync(_ => true);
                _logger.LogInformation($"Stored {countAfterProcessing} services.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error writing down records");
            }

            return awsServices;
        }

    }
}
