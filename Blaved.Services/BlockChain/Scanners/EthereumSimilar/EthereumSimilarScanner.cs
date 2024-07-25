using Blaved.Interfaces.Services.BlockChain;
using Blaved.Objects.Models;

namespace Blaved.Services.BlockChain.Scanners.EthereumSimilar
{
    public class EthereumSimilarScanner : IBlockChainScanner
    {
        private readonly string Network;
        private readonly string MainAsset;
        private readonly IEthereumSimilarScanUsersService _ethereumSimilarScanUsersService;
        public EthereumSimilarScanner(string network, string mainAsset, IEthereumSimilarScanUsersService ethereumSimilarScanUsersService)
        {
            Network = network;
            MainAsset = mainAsset;
            _ethereumSimilarScanUsersService = ethereumSimilarScanUsersService;
        }
        public virtual async Task<List<TransactionDTO>> ScanUserDeposit(HashSet<string> address, HashSet<string> hashs, string Asset)
        {
            if (Asset == MainAsset)
            {
                return await _ethereumSimilarScanUsersService.ScanUsersDeposit(address, hashs, Asset, Network, false);
            }
            return await _ethereumSimilarScanUsersService.ScanUsersDeposit(address, hashs, Asset, Network, true);
        }

    }
}
