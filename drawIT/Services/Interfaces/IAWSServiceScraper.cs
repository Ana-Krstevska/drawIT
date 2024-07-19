using drawIT.Models;

namespace drawIT.Services.Interfaces
{
    public interface IAWSServiceScraper
    {
        Task StartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
        void Dispose();
        Task<List<AWSService>> StoreScrapedAWSServices();
    }
}
