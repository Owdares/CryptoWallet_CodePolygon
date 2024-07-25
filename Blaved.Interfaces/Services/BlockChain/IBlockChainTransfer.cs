using Blaved.Models;

namespace Blaved.Interfaces.Services.BlockChain
{
    public interface IBlockChainTransfer
    {
        public Task<HotTransferModel> TransferCoinToHot(UserModel user, string Asset);
    }
}
