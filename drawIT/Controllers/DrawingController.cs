using drawIT.API.Services.Interfaces;
using drawIT.Models;
using Microsoft.AspNetCore.Mvc;

namespace drawIT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class drawITController : ControllerBase
    {
        private readonly IDrawingRequestService _drawingRequestService;
        private readonly IDatabaseService _databaseService;
        private readonly ILogger<drawITController> _logger;

        public drawITController(ILogger<drawITController> logger, IDrawingRequestService drawingRequestService,
                                IDatabaseService databaseService)
        {
            _logger = logger;
            _drawingRequestService = drawingRequestService;
            _databaseService = databaseService;
        }

        [HttpGet]
        [Route ("GetAzureServices")]
        public async Task<List<AzureService>> GetAzureServices()
        {
            var azureServices = await _databaseService.GetAllAzureServices();
            return azureServices;
        }

        [HttpGet]
        [Route("GetAWSServices")]
        public async Task<List<AWSService>> GetAWSServices()
        {
            var awsServices = await _databaseService.GetAllAWSServices();
            return awsServices;
        }

    }
}
