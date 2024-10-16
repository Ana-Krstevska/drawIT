﻿using drawIT.API.Services.Interfaces;
using drawIT.Database;
using drawIT.Models;
using MongoDB.Driver;

namespace drawIT.API.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly IDbContext _context;
        private readonly ILogger<DatabaseService> _logger;
        public DatabaseService(IDbContext context, ILogger<DatabaseService> logger) 
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> WriteConfigurationToDatabase(ConfigurationRequest configurationRequest)
        {
            try
            {
                await _context.ConfigurationRequests.InsertOneAsync(configurationRequest);
                _logger.LogInformation($"Inserting record: {configurationRequest.Id}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while writing down Configuration request");
                return false;
            }
        }

        public async Task<bool> WriteDrawingToDatabase(DrawingRequest drawingRequest)
        {
            try
            {
                await _context.DrawingRequests.InsertOneAsync(drawingRequest);
                _logger.LogInformation($"Inserting record: {drawingRequest.Id}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while writing down Drawing request");
                return false;
            }
        }

        public async Task<List<AzureService>> GetAllAzureServices()
        {
            return await _context.AzureServices.Find(service => true).ToListAsync();
        }

        public async Task<List<AWSService>> GetAllAWSServices()
        {
            return await _context.AWSServices.Find(service => true).ToListAsync();
        }

        public async Task<List<SuggestionResponse>> GetAllAzureSuggestions()
        {
            return await _context.AzureSuggestions.Find(service => true).ToListAsync();
        }

        public async Task<List<SuggestionResponse>> GetAllAWSSuggestions()
        {
            return await _context.AWSSuggestions.Find(service => true).ToListAsync();
        }

        public async Task<DrawingRequest> GetDrawingById(string id)
        {
            var filter = Builders<DrawingRequest>.Filter.Eq(d => d.Id, id);
            var drawingRequest = await _context.DrawingRequests.Find(filter).FirstOrDefaultAsync();

            return drawingRequest;
        }
    }
}
