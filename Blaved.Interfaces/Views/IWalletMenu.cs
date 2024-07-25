using Telegram.Bot.Types;
using Blaved.Models;
using Blaved.Models.Info;

namespace Blaved.Interfaces.Views
{
    public interface IWalletMenu
    {
        Task<Message?> Wallet(UserModel user, CancellationToken cancellationToken, int currentPage = 1, bool isEdit = true);

        #region Bonus Balance
        Task BonusBalanceToMainBalanceCompleteAnswer(UserModel user, string callbackQueryId, CancellationToken cancellationToken);
        Task BonusBalanceToMainBalanceErrorAnswer(UserModel user, string callbackQueryId, CancellationToken cancellationToken);
        Task<Message?> BonusBalance(UserModel user, CancellationToken cancellationToken, int currentPage = 1, bool isEdit = true);
        #endregion

        #region Withdraw
        Task<Message?> WithdrawCreateWaitNetwork(UserModel user, string Asset, CancellationToken cancellationToken, bool isEdit = true);
        Task<Message?> WithdrawCreateWaitAsset(UserModel user, CancellationToken cancellationToken, int currentPage = 1, bool isEdit = true);
        Task<Message?> WithdrawCreateWaitAddress(UserModel user, string Asset, string network, CancellationToken cancellationToken, bool isEdit = true);
        Task<Message?> WithdrawCreateWaitAmount(UserModel user, CancellationToken cancellationToken, bool isEdit = true);
        Task<Message?> WithdrawConfirm(UserModel user, CancellationToken cancellationToken, bool isEdit = true);
        Task<Message?> WithdrawCreateNotCorrectAddress(UserModel user, CancellationToken cancellationToken, bool isEdit = true);
        Task<Message?> WithdrawCreateNotCorrectAmount(UserModel user, CancellationToken cancellationToken, bool isEdit = true);
        Task WithdrawSentAnswer(UserModel user, string callbackQueryId, CancellationToken cancellationToken);
        Task WithdrawCreateInsufficientBalanceAnswer(UserModel user, InfoForWithdrawModel infoForWithdraw, string callbackQueryId, CancellationToken cancellationToken);
        Task<Message?> WithdrawErrorAlert(UserModel user, CancellationToken cancellationToken, bool isEdit = true);
        Task<Message?> WithdrawCompletedAlert(UserModel user, WithdrawModel withdraw, bool isEdit = true);
        #endregion

        #region Deposit
        Task<Message?> DepositWaitAsset(UserModel user, CancellationToken cancellationToken, int currentPage = 1, bool isEdit = true);
        Task<Message?> DepositWaitNetwork(UserModel user, string Asset, CancellationToken cancellationToken, bool isEdit = true);
        Task<Message?> DepositViewAddress(UserModel user, string Asset, string network, CancellationToken cancellationToken, bool isEdit = true);
        Task<Message?> DepositAlert(UserModel user, DepositModel depositModel, bool isEdit = true);
        #endregion

        #region TransactionHistory
        Task<Message?> WalletTransactionHistory(UserModel user, CancellationToken cancellationToken, bool isEdit = true);
        Task<Message?> WithdrawHistoryWaitNetwork(UserModel user, CancellationToken cancellationToken, bool isEdit = true);
        Task<Message?> DepositHistoryWaitNetwork(UserModel user, CancellationToken cancellationToken, bool isEdit = true);
        Task<Message?> DepositHistoryList(UserModel user, string network, List<DepositModel> history,
            CancellationToken cancellationToken, int currentPage = 1, bool isEdit = true);
        Task<Message?> WithdrawHistoryList(UserModel user, string network, List<WithdrawModel> history,
            CancellationToken cancellationToken, int currentPage = 1, bool isEdit = true);
        Task DepositHistoryNoneAnswer(UserModel user, string callbackQueryId, CancellationToken cancellationToken);
        Task WithdrawHistoryNoneAnswer(UserModel user, string callbackQueryId, CancellationToken cancellationToken);
        Task<Message?> DepositHistoryView(UserModel user, DepositModel transaction, CancellationToken cancellationToken, bool isEdit = true);
        Task<Message?> WithdrawHistoryView(UserModel user, WithdrawModel transaction, CancellationToken cancellationToken, bool isEdit = true);
        #endregion
    }
}
