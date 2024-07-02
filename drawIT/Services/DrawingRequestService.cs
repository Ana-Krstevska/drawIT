using drawIT.API.Services.Interfaces;
using drawIT.Database;
using drawIT.Models;
using HtmlAgilityPack;


namespace drawIT.API.Services
{
    public class DrawingRequestService : IDrawingRequestService
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly IAzureServiceDbContext _context;

        public DrawingRequestService(IAzureServiceDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RegisterDrawingRequestAsync()
        {
            return false;
        }

        public async Task<DrawingRequest> GetDrawingRequestAsync(string id)
        {
            return null;
        }

        public async Task<List<AzureService>> GetCloudServicesAsync()
        {
            var azureServices = new List<AzureService>();

            var response = await client.GetAsync("https://azure.microsoft.com/en-us/products/");
            var pageContents = await response.Content.ReadAsStringAsync();

            var pageDocument = new HtmlDocument();
            pageDocument.LoadHtml(pageContents);

            var headers = pageDocument.DocumentNode.Descendants("h3")
                .Where(h => h.Attributes.Contains("class") && h.Attributes["class"].Value.Contains("h5"))
                .Select(h => h.InnerText);

            foreach (var header in headers)
            {
                azureServices.Add(new AzureService { Name = header });
            }

            return azureServices;
        }

    }
}
