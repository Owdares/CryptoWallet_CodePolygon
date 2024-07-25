using Blaved.Models;

namespace Blaved.Interfaces.Repository
{
    public interface IWithdrawRepository
    {
        Task AddWithdraw(WithdrawModel withdraw);
        Task<List<WithdrawModel>> GetWithdrawList(long userId, string Asset, string network);
        Task<List<WithdrawModel>> GetWithdrawList(long userId, string network);
        Task<WithdrawModel?> GetWithdraw(int num);
    }
}
