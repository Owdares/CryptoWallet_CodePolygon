using Blaved.Objects.Models;

namespace Blaved.Interfaces.Services.BlockChain
{
    public interface IBlockChainScanner
    {
        public Task<List<TransactionDTO>> ScanUserDeposit(HashSet<string> address, HashSet<string> hashs, string Asset);
    }
}
