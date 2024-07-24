using drawIT.Models;

namespace drawIT.API.Services.Interfaces
{
    public interface IDatabaseService
    {
        Task<bool> WriteToDatabase(DrawingRequest drawingRequest);
        Task<List<AzureService>> GetAllAzureServices();
        Task<List<AWSService>> GetAllAWSServices();
    }
}
