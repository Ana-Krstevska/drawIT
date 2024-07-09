﻿using drawIT.Controllers;
using drawIT.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace drawIT.Database
{
    public class AzureServiceDbContext : IAzureServiceDbContext
    {
        private readonly IMongoDatabase _database = null;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AzureServiceDbContext> _logger;

        public AzureServiceDbContext(IConfiguration configuration, ILogger<AzureServiceDbContext> logger)
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
    }
}
