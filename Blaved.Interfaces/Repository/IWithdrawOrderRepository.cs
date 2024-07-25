using Binance.Net.Enums;
using Blaved.Models;

namespace Blaved.Interfaces.Repository
{
    public interface IWithdrawOrderRepository
    {
        Task AddWithdrawOrder(WithdrawOrderModel withdrawOrder);
        Task<WithdrawOrderModel?> GetWithdrawOrder(string idOrder);
        Task<List<WithdrawOrderModel>> GetWithdrawOrderList();
        Task UpdateWithdrawOrderStatus(string idOrder, WithdrawalStatus newStatus);
    }
}
