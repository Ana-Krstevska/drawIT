using drawIT.Models;
using System.Threading.Tasks;

namespace drawIT.Services.Interfaces
{
    public interface IAWSServiceScraper
    {
        Task StartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
        void Dispose();
        Task<List<AWSService>> StoreScrapedAWSServices();
        Task<List<AWSService>> GetAWSCloudServicesAsync();
    }
}
