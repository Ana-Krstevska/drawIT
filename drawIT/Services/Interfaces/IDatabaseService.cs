using drawIT.Models;

namespace drawIT.API.Services.Interfaces
{
    public interface IDatabaseService
    {
        Task<List<AzureService>> GetAllAzureServices();
        Task<List<AWSService>> GetAllAWSServices();
        Task<bool> WriteConfigurationToDatabase(ConfigurationRequest configurationRequest);
        Task<bool> WriteDrawingToDatabase(DrawingRequest drawingRequest);
        Task<List<SuggestionResponse>> GetAllAzureSuggestions();
        Task<List<SuggestionResponse>> GetAllAWSSuggestions();
        Task<DrawingRequest> GetDrawingById(string id);
    }
}
