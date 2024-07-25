using Blaved.Objects.Models;

namespace Blaved.Interfaces.Repository
{
    public interface IExchangeRepository
    {
        Task AddExchange(ExchangeModel exchange);
        Task<List<ExchangeModel>> GetExchangeList(long userId);
        Task<ExchangeModel?> GetExchange(int num);
    }
}
