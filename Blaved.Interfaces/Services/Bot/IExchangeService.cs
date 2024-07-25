using Blaved.Models;
using Blaved.Models.Info;
using Blaved.Objects.Models;

namespace Blaved.Interfaces.Services.Bot
{
    public interface IExchangeService
    {
        Task<Result<ExchangeQuoteModel>> ConvertQuoteRequest(UserModel user, InfoForExchangeModel infoForExchangeModel);
        Task<Result<ExchangeModel>> ConvertQuoteAccept(UserModel user, string quoteId, InfoForExchangeModel infoForExchangeModel);
    }
}
