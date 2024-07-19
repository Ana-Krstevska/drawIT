using drawIT.Models;
using MongoDB.Driver;

namespace drawIT.Database
{
    public interface IDbContext
    {
        IMongoCollection<AzureService> AzureServices { get; }
        IMongoCollection<AWSService> AWSServices { get; }
    }
}
