using drawIT.Models;

namespace drawIT.API.Services.Interfaces
{
    public interface IDatabaseService
    {
        Task<bool> WriteToDatabase(DrawingRequest drawingRequest);
        Task<DrawingRequest> ReadFromDatabase();
    }
}
