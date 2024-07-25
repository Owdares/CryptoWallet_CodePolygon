using Blaved.Interfaces.Services.BlockChain;

namespace Blaved.Services.BlockChain.Scanners.EthereumSimilar
{
    public class BscScanner : EthereumSimilarScanner
    {
        private const string Network = "BSC";
        private const string MainAsset = "BNB";
        public BscScanner(IEthereumSimilarScanUsersService ethereumSimilarUsersScanService)
            : base(Network, MainAsset, ethereumSimilarUsersScanService)
        {
        }
    }
}
