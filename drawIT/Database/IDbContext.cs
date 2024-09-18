using drawIT.Models;
using MongoDB.Driver;

namespace drawIT.Database
{
    public interface IDbContext
    {
        IMongoCollection<AzureService> AzureServices { get; }
        IMongoCollection<AWSService> AWSServices { get; }
        IMongoCollection<ConfigurationRequest> ConfigurationRequests { get; }
        IMongoCollection<DrawingRequest> DrawingRequests { get; }
    }
}
