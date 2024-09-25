using drawIT.API.Services.Interfaces;
using drawIT.Database;
using drawIT.Models;
using Newtonsoft.Json.Linq;
namespace drawIT.API.Services
{
    public class DrawingRequestService : IDrawingRequestService
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly IDbContext _context;
        private readonly IDatabaseService _databaseService;
        private readonly ILogger<DrawingRequestService> _logger;

        public DrawingRequestService(IDbContext context,
            IDatabaseService databaseService,
            ILogger<DrawingRequestService> logger)
        {
            _context = context;
            _logger = logger;
            _databaseService = databaseService;
        }

        public DrawingRequest MapResponseToDrawing(string response)
        {
            JObject json = JObject.Parse(response);
            string content = (string)json["message"]["content"];
            string[] cleanContent = content.Contains("\n\n") ? content.Split("\n\n") : new string[] { content };
            string[] pairs = cleanContent.Length > 1 ? cleanContent[1].Split(';') : cleanContent[0].Split(';');
            List<ServicePair> configuration = new List<ServicePair>();

            foreach (string pair in pairs)
            { 
                string[] services = pair.Split(" to "); 
                configuration.Add(new ServicePair { SourceService = services[0].Trim(), DestinationService = services[1].Trim() });
            }

            DrawingRequest drawingRequest = new DrawingRequest
            {
                Configuration = configuration
            };

            return drawingRequest;
        }

        public async Task<DrawingRequest> WriteDrawingRequestToDatabaseAsync(string response)
        {
            var request = MapResponseToDrawing(response);
            var wasWrittenSuccessfully = await _databaseService.WriteDrawingToDatabase(request);

            if (!wasWrittenSuccessfully)
            {
                throw new InvalidOperationException("Failed to write drawing request to database.");
            }

            return request;
        }

        public async Task<DrawingRequest> GetDrawingRequestAsync(string id)
        {
            return null;
        }
    }
}
