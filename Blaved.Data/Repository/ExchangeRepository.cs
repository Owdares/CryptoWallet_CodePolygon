using Blaved.Data.DataBase;
using Blaved.Interfaces.Repository;
using Blaved.Objects.Models;
using Microsoft.EntityFrameworkCore;

namespace Blaved.Data.Repository
{
    public class ExchangeRepository : IExchangeRepository
    {
        private readonly MyDbContext _dbContext;
        public ExchangeRepository(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddExchange(ExchangeModel exchange)
        {
            await _dbContext.Exchanges.AddAsync(exchange);
        }
        public async Task<List<ExchangeModel>> GetExchangeList(long userId)
        {
            return await _dbContext.Exchanges.Where(c => c.UserId == userId).ToListAsync();
        }
        public async Task<ExchangeModel?> GetExchange(int num)
        {
            return await _dbContext.Exchanges.SingleOrDefaultAsync(u => u.Id == num);
        }
    }
}
