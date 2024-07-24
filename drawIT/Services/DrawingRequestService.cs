using drawIT.API.Services.Interfaces;
using drawIT.Database;
using drawIT.Models;
using drawIT.Services.Interfaces;
namespace drawIT.API.Services
{
    public class DrawingRequestService : IDrawingRequestService
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly IAzureServiceScraper _azureServiceScraper;
        private readonly IDbContext _context;
        private readonly ILogger<DrawingRequestService> _logger;
        private readonly IAWSServiceScraper _awsServiceScraper;

        public DrawingRequestService(IDbContext context,
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
    }
}
