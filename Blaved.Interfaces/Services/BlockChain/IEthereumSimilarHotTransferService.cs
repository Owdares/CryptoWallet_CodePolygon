using Blaved.Models;

namespace Blaved.Interfaces.Services.BlockChain
{
    public interface IEthereumSimilarHotTransferService
    {
        public Task<HotTransferModel> TransferToHot(UserModel userModel,string network, string Asset, bool isToken);
    }
}
