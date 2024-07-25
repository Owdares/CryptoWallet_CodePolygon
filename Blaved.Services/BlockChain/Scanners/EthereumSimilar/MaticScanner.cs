using Blaved.Interfaces.Services.BlockChain;

namespace Blaved.Services.BlockChain.Scanners.EthereumSimilar
{
    public class MaticScanner : EthereumSimilarScanner
    {
        private const string Network = "MATIC";
        private const string MainAsset = "MATIC";
        public MaticScanner(IEthereumSimilarScanUsersService ethereumSimilarUsersScanService)
            : base(Network, MainAsset, ethereumSimilarUsersScanService)
        {
        }
    }
}
