using drawIT.Models.Enums;

namespace drawIT.Models
{
    public class SuggestionRequest
    {
        public CloudProvider Cloud { get; set; }
        public List<string>? CloudServices { get; set; }
    }
}
