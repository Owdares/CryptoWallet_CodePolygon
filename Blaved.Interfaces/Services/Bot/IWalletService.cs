using Blaved.Models;
using Blaved.Models.Info;

namespace Blaved.Interfaces.Services.Bot
{
    public interface IWalletService
    {
        Task<bool> BonusBalanceToBalance(UserModel user);

        Task WithdrawConfirm(UserModel user, InfoForWithdrawModel infoForWithdrawModel);
        Task WithdrawValidate();
        Task<List<WithdrawModel>> WithdrawValidateOrder();

        Task DepositScanTransaction();
    }
}
