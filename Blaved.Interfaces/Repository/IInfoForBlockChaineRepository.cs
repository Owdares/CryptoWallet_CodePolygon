using Blaved.Models.Info;

namespace Blaved.Interfaces.Repository
{
    public interface IInfoForBlockChainRepository
    {
        Task<InfoForBlockChainModel?> GetInfoForBlockChaine(string Asset, string network);
        Task UpdateLastScanBlock(string Asset, string network, long lastScanBlock);
    }
}
