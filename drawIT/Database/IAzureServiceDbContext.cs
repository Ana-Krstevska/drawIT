using drawIT.Models;
using MongoDB.Driver;

namespace drawIT.Database
{
    public interface IAzureServiceDbContext
    {
        IMongoCollection<AzureService> AzureServices { get; }
        IMongoCollection<AWSService> AWSServices { get; }
    }
}
