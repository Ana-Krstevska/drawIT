using drawIT.API.Services.Interfaces;
using drawIT.Database;
using drawIT.Models;
using MongoDB.Driver;

namespace drawIT.API.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly IDbContext _context;
        public DatabaseService(IDbContext context) 
        {
            _context = context;
        }

        public async Task<bool> WriteToDatabase(DrawingRequest drawingRequest)
        {
            return false;
        }

        public async Task<List<AzureService>> GetAllAzureServices()
        {
            return await _context.AzureServices.Find(service => true).ToListAsync();
        }

        public async Task<List<AWSService>> GetAllAWSServices()
        {
            return await _context.AWSServices.Find(service => true).ToListAsync();
        }
    }
}
