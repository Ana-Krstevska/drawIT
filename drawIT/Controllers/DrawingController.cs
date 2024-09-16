using drawIT.API.Services.Interfaces;
using drawIT.Models;
using drawIT.Services;
using drawIT.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace drawIT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class drawITController : ControllerBase
    {
        private readonly IDrawingRequestService _drawingRequestService;
        private readonly ILlamaService _llamaService;
        private readonly IDatabaseService _databaseService;
        private readonly ILogger<drawITController> _logger;

        public drawITController(ILogger<drawITController> logger, IDrawingRequestService drawingRequestService,
                                IDatabaseService databaseService, ILlamaService llamaService)
        {
            _logger = logger;
            _drawingRequestService = drawingRequestService;
            _databaseService = databaseService;
            _llamaService = llamaService;
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

        [HttpPost("ProcessPrompt")]
        public async Task<IActionResult> SendPrompt([FromBody] ConfigurationRequest request)
        {
            if (string.IsNullOrEmpty(request.UserDescription))
            {
                return BadRequest("Prompt is required.");
            }

            var wroteRecord = await _databaseService.WriteConfigurationToDatabase(request);
            
            if(wroteRecord)
            {
                await _llamaService.SendPromptToLlamaApiAsync(request.UserDescription);
                return Accepted();
            }

            return StatusCode(500, "Internal server error, failed to write to database.");
        }
    }
}
