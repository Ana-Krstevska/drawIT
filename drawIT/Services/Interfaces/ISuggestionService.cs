using drawIT.Models;

namespace drawIT.Services.Interfaces
{
    public interface ISuggestionService
    {
        Task<SuggestionResponse> CheckSuggestions(SuggestionRequest request);
    }
}
