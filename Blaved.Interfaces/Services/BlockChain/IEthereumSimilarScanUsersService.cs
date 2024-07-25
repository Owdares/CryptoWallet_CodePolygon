using Blaved.Objects.Models;

namespace Blaved.Interfaces.Services.BlockChain
{
    public interface IEthereumSimilarScanUsersService
    {
        public Task<List<TransactionDTO>> ScanUsersDeposit(HashSet<string> addresses, HashSet<string> transfersHash, string Asset, string network, bool isToken);
    }
}
