using Blaved.Models.Info;

namespace Bleved.Interfaces.Services
{
    public interface IInfoService
    {
        Task<InfoForWithdrawModel> GetInfoForWithdraw(string Asset, string network);
        Task<InfoForConvertModel> GetInfoForConvert(string fromAsset, string toAsset);
        Task<List<InfoForConvertModel>> GetInfoForConvert(string fromAsset);
        Task<InfoForExchangeModel> GetInfoForExchange(string fromAsset, string toAsset);
        Task<List<InfoForExchangeModel>> GetInfoForExchange(string fromAsset);
        Task<decimal> GetCoinPriceUSDT(string Asset);
        Task<InfoForCheckModel> GetInfoForCheck(string Asset);
        Task<InfoForBlavedPayIDModel> GetInfoForBlavedPayID(string Asset);
        Task<InfoForDepositModel> GetInfoForDeposit(string Asset, string network);
    }
}
