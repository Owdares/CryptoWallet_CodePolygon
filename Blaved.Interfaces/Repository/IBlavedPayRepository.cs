using Blaved.Models;

namespace Blaved.Interfaces.Repository
{
    public interface IBlavedPayIDRepository
    {
        Task AddBlavedPayIDTransfer(BlavedPayIDTransferModel blavedPayIDTransfer);
        Task<List<BlavedPayIDTransferModel>> GetBlavedPayIDTransferList(long userId);
        Task<BlavedPayIDTransferModel?> GetBlavedPayIDTransfer(int num);
    }
}
