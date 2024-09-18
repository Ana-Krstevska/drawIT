using drawIT.Models;
using MongoDB.Driver;

namespace drawIT.Database
{
    public class DbContext : IDbContext
    {
        private readonly IMongoDatabase _database = null;
        private readonly IConfiguration _configuration;
        private readonly ILogger<DbContext> _logger;

        public DbContext(IConfiguration configuration, ILogger<DbContext> logger)
        {
            _configuration = configuration;
            _logger = logger;

            var connectionString = Environment.GetEnvironmentVariable("MongoDBConnectionString");
            var client = new MongoClient(connectionString);
            if (client != null)
                _database = client.GetDatabase(_configuration.GetValue<string>("Database"));
        }

        public IMongoCollection<AzureService> AzureServices
        {
            get
            {
                try
                {
                    var collectionName = _configuration.GetValue<string>("AzureService");
                    _logger.LogInformation($"Retrieving collection: {collectionName}");
                    return _database.GetCollection<AzureService>(collectionName);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while retrieving AzureServices collection");
                    throw;
                }
            }
        }

        public IMongoCollection<AWSService> AWSServices
        {
            get
            {
                try
                {
                    var collectionName = _configuration.GetValue<string>("AWSService");
                    _logger.LogInformation($"Retrieving collection: {collectionName}");
                    return _database.GetCollection<AWSService>(collectionName);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while retrieving AzureServices collection");
                    throw;
                }
            }
        }

        public IMongoCollection<ConfigurationRequest> ConfigurationRequests
        {
            get
            {
                try
                {
                    var collectionName = _configuration.GetValue<string>("ConfigurationRequest");
                    _logger.LogInformation($"Retrieving collection: {collectionName}");
                    return _database.GetCollection<ConfigurationRequest>(collectionName);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while retrieving DrawingRequest collection");
                    throw;
                }
            }
        }

        public IMongoCollection<DrawingRequest> DrawingRequests
        {
            get
            {
                try
                {
                    var collectionName = _configuration.GetValue<string>("DrawingRequest");
                    _logger.LogInformation($"Retrieving collection: {collectionName}");
                    return _database.GetCollection<DrawingRequest>(collectionName);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while retrieving DrawingRequest collection");
                    throw;
                }
            }
        }
    }
}
