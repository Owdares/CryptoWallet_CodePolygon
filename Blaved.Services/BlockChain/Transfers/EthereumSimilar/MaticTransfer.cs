using Blaved.Interfaces.Services.BlockChain;

namespace Blaved.Services.BlockChain.Transfers.EthereumSimilar
{
    public class MaticTransfer : EthereumSimilarTransfer
    {
        private const string Network = "MATIC";
        private const string MainAsset = "MATIC";

        public MaticTransfer(IEthereumSimilarHotTransferService ethereumSimilarHotService)
        : base(Network, MainAsset, ethereumSimilarHotService)
        {
        }
    }
}
