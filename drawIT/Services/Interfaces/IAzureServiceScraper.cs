using drawIT.Models;

namespace drawIT.Services.Interfaces
{
    public interface IAzureServiceScraper
    {
        Task StartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
        void Dispose();
        Task<List<AzureService>> StoreScrapedAzureServices();
        Task<List<AzureService>> GetCloudServicesAsync();
    }
}
