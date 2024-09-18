using drawIT.Models;

namespace drawIT.API.Services.Interfaces
{
    public interface IDrawingRequestService
    {
        Task<DrawingRequest> WriteDrawingRequestToDatabaseAsync(string response);
        Task<DrawingRequest> GetDrawingRequestAsync(string id);
        DrawingRequest MapResponseToDrawing(string response);
    }
}
