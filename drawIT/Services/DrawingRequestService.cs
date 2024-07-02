using drawIT.API.Services.Interfaces;
using drawIT.Models;
using HtmlAgilityPack;


namespace drawIT.API.Services
{
    public class DrawingRequestService : IDrawingRequestService
    {
        private static readonly HttpClient client = new HttpClient();
        public DrawingRequestService() { }

        public async Task<bool> RegisterDrawingRequestAsync()
        {
            return false;
        }

        public async Task<DrawingRequest> GetDrawingRequestAsync(string id)
        {
            return null;
        }

        public async Task<string[]> GetCloudServicesAsync()
        {
            var response = await client.GetAsync("https://azure.microsoft.com/en-us/products/");
            var pageContents = await response.Content.ReadAsStringAsync();

            var pageDocument = new HtmlDocument();
            pageDocument.LoadHtml(pageContents);

            var headers = pageDocument.DocumentNode.Descendants("h3")
                .Where(h => h.Attributes.Contains("class") && h.Attributes["class"].Value.Contains("h5"))
                .Select(h => h.InnerText)
                .ToArray();

            foreach (var header in headers)
            {
                Console.WriteLine(header);
            }
            return null;
        }
    }
}
