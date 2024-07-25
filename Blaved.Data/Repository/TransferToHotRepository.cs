using Blaved.Data.DataBase;
using Blaved.Interfaces.Repository;
using Blaved.Models;

namespace Blaved.Data.Repository
{
    public class TransferToHotRepository : ITransferToHotRepository
    {
        private readonly MyDbContext _dbContext;
        public TransferToHotRepository(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddTransferToHot(HotTransferModel hotTransfer)
        {
            await _dbContext.HotTransfers.AddAsync(hotTransfer);
        }
    }
}
