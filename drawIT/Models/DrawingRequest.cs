namespace drawIT.Models
{
    public class DrawingRequest
    {
        public DrawingRequest() 
        {
            Id = Guid.NewGuid().ToString();
        }
        public string? Id { get; set; }
        public List<ServicePair>? Configuration { get; set; }
    }
}
