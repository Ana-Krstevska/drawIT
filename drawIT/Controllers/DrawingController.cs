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

        private readonly ILogger<drawITController> _logger;

        public drawITController(ILogger<drawITController> logger, IDrawingRequestService drawingRequestService)
        {
            _logger = logger;
            _drawingRequestService = drawingRequestService;
        }

        [HttpGet(Name = "GetCloudServices")]
        public async Task<ActionResult<List<AzureService>>> GetCloudServices()
        {
            try
            {
                List<AWSService> awsCloudServices = await _drawingRequestService.GetAWSCloudServicesAsync();
                List<AzureService> cloudServices = await _drawingRequestService.GetCloudServicesAsync();
                if (cloudServices == null)
                {
                    _logger.LogError("No cloud services found");
                    return NotFound("No cloud services found");
                }

                return cloudServices;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving cloud services");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
