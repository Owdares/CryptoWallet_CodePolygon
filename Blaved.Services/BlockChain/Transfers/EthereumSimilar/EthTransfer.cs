using Blaved.Interfaces.Services.BlockChain;

namespace Blaved.Services.BlockChain.Transfers.EthereumSimilar
{
    public class EthTransfer : EthereumSimilarTransfer
    {
        private const string Network = "ETH";
        private const string MainAsset = "ETH";

        public EthTransfer(IEthereumSimilarHotTransferService ethereumSimilarHotService)
        : base(Network, MainAsset, ethereumSimilarHotService)
        {
        }
    }
}
