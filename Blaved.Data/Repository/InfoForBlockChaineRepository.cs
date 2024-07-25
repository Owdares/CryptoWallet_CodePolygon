using Blaved.Data.DataBase;
using Blaved.Interfaces.Repository;
using Blaved.Models.Info;
using Microsoft.EntityFrameworkCore;

namespace Blaved.Data.Repository
{
    public class InfoForBlockChainRepository : IInfoForBlockChainRepository
    {
        private readonly MyDbContext _dbContext;
        public InfoForBlockChainRepository(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<InfoForBlockChainModel?> GetInfoForBlockChaine(string Asset, string network)
        {
            return await _dbContext.InfoForBlockChaines.SingleOrDefaultAsync(u => u.Asset == Asset && u.Network == network);
        }
        public async Task UpdateLastScanBlock(string Asset, string network, long lastScanBlock)
        {
            var infoForBlockChain = await _dbContext.InfoForBlockChaines.SingleOrDefaultAsync(u => u.Asset == Asset && u.Network == network);
            if (infoForBlockChain == null)
            {
                throw new Exception();
            }
            infoForBlockChain.LastScanBlock = lastScanBlock;

        }
    }
}
