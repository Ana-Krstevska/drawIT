namespace drawIT.Models
{
    public class DrawingRequest
    {
        public string? Id { get; set; }
        public List<ServicePair>? Configuration { get; set; }
    }
}
