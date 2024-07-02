using drawIT.API.Services;
using drawIT.Database;
using MongoDB.Driver;

namespace drawIT.Services
{
    public class AzureServiceScraper : IHostedService, IDisposable
    {
        private readonly IAzureServiceDbContext _context;
        private readonly DrawingRequestService _drawingRequestService;
        private Timer? _timer;
        private readonly ILogger<AzureServiceScraper> _logger;

        public AzureServiceScraper(IAzureServiceDbContext context, DrawingRequestService drawingRequestService, 
                                    ILogger<AzureServiceScraper> logger)
        {
            _context = context;
            _drawingRequestService = drawingRequestService;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Azure service scraper is starting.");

            _timer = new Timer(ScrapeAzureServices, null, TimeSpan.Zero, TimeSpan.FromDays(7)); // Run once a week  

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

        private async void ScrapeAzureServices(object? state)
        {
            var azureServices = await _drawingRequestService.GetCloudServicesAsync();

            foreach (var azureService in azureServices)
            {
                var existingService = _context.AzureServices.Find(service => service.Name == azureService.Name).FirstOrDefault();

                if (existingService == null)
                {
                    _context.AzureServices.InsertOne(azureService);
                }
            }
        }
    }
}
