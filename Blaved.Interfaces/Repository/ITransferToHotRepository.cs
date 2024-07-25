using Blaved.Models;

namespace Blaved.Interfaces.Repository
{
    public interface ITransferToHotRepository
    {
        Task AddTransferToHot(HotTransferModel hotTransfer);
    }
}
