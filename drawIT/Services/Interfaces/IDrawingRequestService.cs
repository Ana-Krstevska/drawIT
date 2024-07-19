using drawIT.Models;

namespace drawIT.API.Services.Interfaces
{
    public interface IDrawingRequestService
    {
        Task<bool> RegisterDrawingRequestAsync();
        Task<DrawingRequest> GetDrawingRequestAsync(string id);
        Task<List<AzureService>> GetCloudServicesAsync();
        Task<List<AWSService>> GetAWSCloudServicesAsync();
    }
}
