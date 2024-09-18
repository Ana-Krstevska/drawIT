using drawIT.Models;

namespace drawIT.Services.Interfaces
{
    public interface ILlamaService
    {
        Task<DrawingRequest> SendPromptToLlamaApiAsync(string prompt);
    }
}
