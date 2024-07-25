using Blaved.Models;

namespace Blaved.Interfaces.Services.Bot
{
    public interface IBlavedPayService
    {
        Task<BlavedPayIDTransferModel> BlavedPayIDTransferConfirm(UserModel user);
    }
}
