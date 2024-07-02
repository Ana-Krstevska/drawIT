using drawIT.API.Services.Interfaces;
using drawIT.Models;

namespace drawIT.API.Services
{
    public class DatabaseService : IDatabaseService
    {
        public DatabaseService() { }
        public async Task<bool> WriteToDatabase(DrawingRequest drawingRequest)
        {
            return false;
        }

        public async Task<DrawingRequest> ReadFromDatabase()
        {
            return null;
        }
    }
}
