namespace drawIT.Models
{
    public class DrawingRequest
    {
        public string? RequestId { get; set; }
        public string? CloudProvider { get; set; }
        public string[]? CloudServices { get; set; }
        public string? Description { get; set; }
    }
}
