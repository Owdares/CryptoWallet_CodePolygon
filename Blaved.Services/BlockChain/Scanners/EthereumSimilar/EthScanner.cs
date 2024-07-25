using Blaved.Interfaces.Services.BlockChain;

namespace Blaved.Services.BlockChain.Scanners.EthereumSimilar
{
    public class EthScanner : EthereumSimilarScanner
    {
        private const string Network = "ETH";
        private const string MainAsset = "ETH";
        public EthScanner(IEthereumSimilarScanUsersService ethereumSimilarUsersScanService) 
            : base(Network, MainAsset, ethereumSimilarUsersScanService)
        {
        }
    }
}
