using drawIT.Models;

namespace drawIT.API.Services.Interfaces
{
    public interface IDrawingRequestService
    {
        Task<bool> RegisterDrawingRequestAsync(string response);
        Task<DrawingRequest> GetDrawingRequestAsync(string id);
        DrawingRequest MapResponseToDrawing(string response);
    }
}
