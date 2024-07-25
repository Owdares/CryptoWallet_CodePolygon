using Binance.Net.Enums;
using Binance.Net.Interfaces.Clients;
using Binance.Net.Objects.Models.Spot;
using Binance.Net.Objects.Models.Spot.Convert;
using Blaved.Interfaces.Services.Binance;
using Blaved.Models.Info;
using Blaved.Objects.Models;
using Blaved.Objects.Models.Configurations;
using CryptoExchange.Net.CommonObjects;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nethereum.Contracts.QueryHandlers.MultiCall;
using Newtonsoft.Json;
using Serilog;
using Serilog.Core;
using Telegram.Bot.Types;

namespace BlevedCrypto.Service.Binance
{
    public class BinanceService : IBinanceService
    {
        private readonly IBinanceRestClient _binanceRestClient;
        private readonly AppConfig _appConfig;
        private readonly ILogger<BinanceService> _logger;
        public BinanceService(IBinanceRestClient binanceRestClient, IOptions<AppConfig> appConfig, ILogger<BinanceService> logger)
        {
            _logger = logger;
            _appConfig = appConfig.Value;
            _binanceRestClient = binanceRestClient;
        } 

        public async Task<Result<BinanceConvertQuote>> ConvertQuote(string fromAsset, string toAsset, decimal amount)
        {
            _logger.LogInformation("Request to ConvertQuote - {@Request}", new { fromAsset, toAsset, amount });

            var result = await _binanceRestClient.SpotApi.Trading.ConvertQuoteRequestAsync(fromAsset, toAsset, amount);

            _logger.LogInformation("Response received for ConvertQuote - {@Response}", result.Success);

            if (!result.Success)
            {
                _logger.LogError("Error while fetching ConvertQuote: {@Response}", result.Error);
                return new Result<BinanceConvertQuote>(false, null);
            }

            _logger.LogDebug("Detailed response received for ConvertQuote - {@Response}", result.Data);
            
            return new Result<BinanceConvertQuote>(true, result.Data);
        }
        public async Task<Result<BinanceConvertResult>> ConvertQuoteAccept(string quoteId)
        {
            _logger.LogInformation("Request to ConvertQuoteAccept - {@Request}", new { quoteId });

            var result = await _binanceRestClient.SpotApi.Trading.ConvertAcceptQuoteAsync(quoteId);

            _logger.LogInformation("Response received for ConvertQuoteAccept - {@Response}", result.Success);

            if (!result.Success)
            {
                _logger.LogError("Error while fetching ConvertQuoteAccept: {@Response}", result.Error);
                return new Result<BinanceConvertResult>(false, null);
            }

            _logger.LogDebug("Detailed response received for ConvertQuoteAccept - {@Response}", result.Data);

            return new Result<BinanceConvertResult>(true, result.Data);
        }
        public async Task<Result<BinanceConvertOrderStatus>> GetConvertOrderStatus(string orderId)
        {
            _logger.LogInformation("Request to GetConvertOrderStatus - {@Request}", new { orderId });

            var result = await _binanceRestClient.SpotApi.Trading.GetConvertOrderStatusAsync(orderId);

            _logger.LogInformation("Response received for GetConvertOrderStatus - {@Response}", result.Success);

            if (!result.Success)
            {
                _logger.LogError("Error while fetching GetConvertOrderStatus: {@Response}", result.Error);
                return new Result<BinanceConvertOrderStatus>(false, null);
            }

            _logger.LogDebug("Detailed response received for GetConvertOrderStatus - {@Response}", result.Data);

            return new Result<BinanceConvertOrderStatus>(true, result.Data);
        }

        public async Task<Result<List<InfoForConvertModel>>> GetCoinConvertInfo()
        {
            _logger.LogInformation("Request to GetCoinConvertInfo");

            var result = await _binanceRestClient.SpotApi.ExchangeData.GetConvertListAllPairsAsync();

            _logger.LogInformation("Response received for GetCoinConvertInfo - {@Response}", result.Success);

            if (!result.Success)
            {
                _logger.LogError("Error while fetching GetCoinConvertInfo: {@Response}", result.Error);
                return new Result<List<InfoForConvertModel>>(false, default);
            }

            var tokens = _appConfig.AssetConfiguration.CoinList;
            var filteredPairs = result.Data.Where(pair => tokens.Contains(pair.QuoteAsset) && tokens.Contains(pair.BaseAsset))
                .Select(x => new InfoForConvertModel()
                {
                    FromAsset = x.QuoteAsset, 
                    ToAsset = x.BaseAsset,
                    MaxAmount = x.QuoteAssetMaxQuantity,
                    MinAmount = x.QuoteAssetMinQuantity,
                    ConvertInternalFee = _appConfig.FunctionConfiguration.ExchangeInternalFee[x.QuoteAsset]
                })
                .ToList();

            _logger.LogDebug("Detailed response received for GetCoinConvertInfo - {@Response}", filteredPairs);

            return new Result<List<InfoForConvertModel>>(true, filteredPairs);
        }
        
        public async Task<Result<string>> SendCoin(string Asset, string network, string address, decimal amount)
        {
            _logger.LogInformation("Request to SendCoin - {@Request}", new { Asset, network, address, amount });

            var result = await _binanceRestClient.SpotApi.Account.WithdrawAsync
                (
                   asset: Asset,
                   address: address,
                   quantity: amount,
                   network: network
                );

            _logger.LogInformation("Response received for SendCoin - {@Response}", new { result.Success });

            if (!result.Success || result.Data.Id == null)
            {
                _logger.LogError("Error while fetching SendCoin: {@Response}", result.Error);
                return new Result<string>(false, null);
            }

            _logger.LogDebug("Detailed response received for SendCoin - {@Response}", result.Data);

            return new Result<string>(true, result.Data.Id);
        }
        public async Task<Result<List<BinanceWithdrawal>>> GetWithdrawHistory()
        {
            _logger.LogInformation("Request to GetWithdrawHistory");

            var result = await _binanceRestClient.SpotApi.Account.GetWithdrawalHistoryAsync();

            _logger.LogInformation("Response received for GetWithdrawHistory - {@Response}", result.Success);

            if (!result.Success)
            {
                _logger.LogError("Error while fetching GetWithdrawHistory: {@Response}", result.Error);
                return new Result<List<BinanceWithdrawal>>(false, null);
            }

            _logger.LogDebug("Detailed response received for GetWithdrawHistory - {@Response}", result.Data);

            return new Result<List<BinanceWithdrawal>>(true, result.Data.ToList());
        }
        public async Task<Result<Dictionary<string, decimal>>> GetCoinPriceUSD()
        {
            _logger.LogInformation("Request to GetCoinPriceUSD");

            var result = await _binanceRestClient.SpotApi.ExchangeData.GetPricesAsync(_appConfig.AssetConfiguration.CoinSymbolsUSD);

            _logger.LogInformation("Response received for GetCoinPriceUSD - {@Response}", result.Success);

            if (!result.Success)
            {
                _logger.LogError("Error while fetching GetCoinPriceUSD: {@Response}", result.Error);
                return new Result<Dictionary<string, decimal>>(false, null);
            }
            var coinPrice = result.Data.ToDictionary(coin => coin.Symbol, coin => coin.Price);

            _logger.LogDebug("Detailed response received for GetCoinPriceUSD - {@Response}", result.Data);

            return new Result<Dictionary<string, decimal>>(true, coinPrice);
        }
        public async Task<Result<List<InfoForWithdrawModel>>> GetCoinInfo()
        {
            _logger.LogInformation("Request to GetCoinInfo");

            var result = await _binanceRestClient.SpotApi.Account.GetUserAssetsAsync();

            _logger.LogInformation("Response received for GetCoinInfo - {@Response}", result.Success);

            if (!result.Success)
            {
                _logger.LogError("Error while fetching GetCoinInfo: {@Response}", result.Error);
                return new Result<List<InfoForWithdrawModel>>(false, null);
            }

            var localAsset = result.Data
                .Where(x => _appConfig.AssetConfiguration.CoinList.Contains(x.Asset))
                .SelectMany(x =>
                {
                    return x.NetworkList
                           .Where(network => _appConfig.AssetConfiguration.NetworkListByCoin[x.Asset].Any(myChain => network.Network.Contains(myChain)))
                           .Select(network => new InfoForWithdrawModel
                           {
                               Network = network.Network,
                               Asset = x.Asset,
                               MaximalSum = network.WithdrawMax,
                               MinimalSum = network.WithdrawMin,
                               WithdrawFee = network.WithdrawFee,
                               WithdrawInternalFee = _appConfig.FunctionConfiguration.WithdrawInternalFee[network.Network][x.Asset],
                               WithdrawCombineFee = network.WithdrawFee + _appConfig.FunctionConfiguration.WithdrawInternalFee[network.Network][x.Asset]

                           });
                }).ToList();

            _logger.LogDebug("Detailed response received for GetCoinInfo - {@Response}", localAsset);

            return new Result<List<InfoForWithdrawModel>>(true, localAsset);
        }
    }
}
