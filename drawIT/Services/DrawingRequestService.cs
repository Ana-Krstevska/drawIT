using drawIT.API.Services.Interfaces;
using drawIT.Database;
using drawIT.Models;
using drawIT.Services.Interfaces;
using MongoDB.Bson;
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
            string content = (string)json.SelectToken("message.content");
            string[] pairs = content.Split(','); 
            List<ServicePair> configuration = new List<ServicePair>();
 
            foreach (string pair in pairs)
            { 
                string[] services = pair.Split("to"); 
                configuration.Add(new ServicePair { SourceService = services[0].Trim(), DestinationService = services[1].Trim() });
            }

            DrawingRequest drawingRequest = new DrawingRequest
            {
                Id = Guid.NewGuid().ToString(),
                Configuration = configuration
            };

            return drawingRequest;
        }

        public async Task<bool> RegisterDrawingRequestAsync(string response)
        {
            DrawingRequest request = MapResponseToDrawing(response);
            var wroteRequest = await _databaseService.WriteDrawingToDatabase(request);
            if(wroteRequest)
            {
                return true;
            }

            return false;
        }

        public async Task<DrawingRequest> GetDrawingRequestAsync(string id)
        {
            return null;
        }
    }
}
