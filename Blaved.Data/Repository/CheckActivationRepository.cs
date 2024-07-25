using Blaved.Data.DataBase;
using Blaved.Interfaces.Repository;
using Blaved.Models;
using Microsoft.EntityFrameworkCore;

namespace Blaved.Data.Repository
{
    public class CheckActivatedRepository : ICheckActivatedRepository
    {
        private readonly MyDbContext _dbContext;
        public CheckActivatedRepository(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddCheckActivated(CheckActivatedModel checkActivated)
        {
            await _dbContext.CheckActivateds.AddAsync(checkActivated);
        }
        public async Task<CheckActivatedModel?> GetCheckActivated(long userId, string url)
        {
            return await _dbContext.CheckActivateds.SingleOrDefaultAsync(u => u.UserId == userId && u.Check.Url == url);
        }
    }
}
