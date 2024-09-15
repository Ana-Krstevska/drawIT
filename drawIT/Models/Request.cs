using drawIT.Models.Enums;

namespace drawIT.Models
{
    public class Request
    {
        public CloudProvider Cloud {  get; set; }
        public List<string>? Services { get; set; }
        public string? UserDescription { get; set; }
    }
}
