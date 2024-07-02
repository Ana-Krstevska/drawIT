using drawIT.Models;
using MongoDB.Driver;

namespace drawIT.Database
{
    public class AzureServiceDbContext : IAzureServiceDbContext
    {
        private readonly IMongoDatabase _database = null;

        public AzureServiceDbContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("AzureServiceDb"));
            if (client != null)
                _database = client.GetDatabase("AzureServiceDb");
        }

        public IMongoCollection<AzureService> AzureServices
        {
            get
            {
                return _database.GetCollection<AzureService>("AzureService");
            }
        }
    }
}
