using drawIT.API.Services.Interfaces;
using drawIT.Models;
using drawIT.Services.Interfaces;

namespace drawIT.Services
{
    public class SuggestionService : ISuggestionService
    {
        private readonly IDatabaseService _databaseService;

        public SuggestionService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<SuggestionResponse> CheckSuggestions(SuggestionRequest request)
        {
            var suggestions = request.Cloud == Models.Enums.CloudProvider.Azure
                ? await _databaseService.GetAllAzureSuggestions()
                : await _databaseService.GetAllAWSSuggestions();

            var defaultSuggestionResponse = new SuggestionResponse() 
            { 
                Name = "No suggestion available"
            };

            foreach (var suggestion in suggestions)
            {
                if (request.CloudServices != null &&
                    suggestion.CloudServices != null &&
                    request.CloudServices.All(r => suggestion.CloudServices.Contains(r)))
                {
                    return suggestion;

                }
            }

            return defaultSuggestionResponse;
        }
    }
}
