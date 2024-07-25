using Binance.Net.Enums;
using Blaved.Interfaces;
using Blaved.Interfaces.Services.Binance;
using Blaved.Interfaces.Services.BlockChain;
using Blaved.Interfaces.Services.Bot;
using Blaved.Models;
using Blaved.Models.Info;
using Blaved.Objects.Models.Configurations;
using Blaved.Objects.Models;
using Blaved.Utility;
using Bleved.Interfaces.Services;
using Microsoft.Extensions.Logging;
using NBitcoin;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog.Context;

namespace Blaved.Services.Bot
{
    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBinanceService _binanceService;
        private readonly IBlockChainScannerFacade _blockChainScannerFacade;
        private readonly IBlockChainTransferFacade _blockChainTransferFacade;
        private readonly ILogger<WalletService> _logger;
        private readonly IBotMenu _botMenu;
        private readonly IInfoService _infoService;
        private readonly AppConfig _appConfig;

        public WalletService(IUnitOfWork unitOfWork, IBinanceService binanceService, ILogger<WalletService> logger, 
            IBotMenu botMenu, IBlockChainScannerFacade blockChainScannerFacade, IBlockChainTransferFacade blockChainTransferFacade,
            IInfoService infoService, IOptions<AppConfig> appConfig)
        {
            _unitOfWork = unitOfWork;
            _binanceService = binanceService;
            _botMenu = botMenu;
            _logger = logger;
            _botMenu = botMenu;
            _blockChainScannerFacade = blockChainScannerFacade;
            _blockChainTransferFacade = blockChainTransferFacade;
            _infoService = infoService;
            _appConfig = appConfig.Value;
        }

        #region Bonus Balance
        public async Task<bool> BonusBalanceToBalance(UserModel user)
        {
            int x = 0;
            foreach (var coin in _appConfig.AssetConfiguration.CoinList)
            {
                var coinBonusBalance = user.BonusBalanceModel.GetBalance(coin);
                if (coinBonusBalance > 0)
                {
                    await _unitOfWork.BonusBalanceRepository.SubtractFromBonusBalance(user.UserId, coinBonusBalance, coin);
                    await _unitOfWork.BalanceRepository.AddToBalance(user.UserId, coinBonusBalance, coin);
                    x++;
                }
            }
            if(x > 0)
            {
                await _unitOfWork.SaveChanges();

                _logger.LogInformation($"The bonus balance has been transferred to the main balance");
                return true;
            }
            return false;
        }
        #endregion

        #region Withdraw
        public async Task WithdrawConfirm(UserModel user, InfoForWithdrawModel infoForWithdrawModel)
        {
            decimal amount = user.MessagesWithdrawModel.Amount;
            string asset = user.MessagesWithdrawModel.Asset;
            string network = user.MessagesWithdrawModel.Network;
            string address = user.MessagesWithdrawModel.Address;

            decimal feeInternal = infoForWithdrawModel.WithdrawInternalFee;
            decimal feeBinance = infoForWithdrawModel.WithdrawFee;
            decimal combineFee = infoForWithdrawModel.WithdrawCombineFee;

            decimal amountForSend = (amount + feeBinance).AmountRound();
            decimal amountForSubtract = (amount + combineFee).AmountRound();

            var orderId = await _binanceService.SendCoin(asset, network, address, amountForSend);
            if (!orderId.Status || orderId.Data == null)
            {
                throw new Exception("Sending coins from Binance failed - Status = false || Data = null");
            }
            var orderModel = new WithdrawOrderModel
            {
                UserId = user.UserId,
                IdOrder = orderId.Data,
                AddressTo = address,
                Amount = amount,
                Asset = asset,
                ChargeToCapital = feeInternal,
                Fee = combineFee,
                Network = network,
                Status = WithdrawalStatus.Processing,
            };
            await _unitOfWork.BalanceRepository.SubtractFromBalance(user.UserId, amountForSubtract, asset);
            await _unitOfWork.WithdrawOrderRepository.AddWithdrawOrder(orderModel);
            await _unitOfWork.SaveChanges();

            _logger.LogDebug("Coin sent {@Order}", JsonConvert.SerializeObject(orderModel, Formatting.Indented));
        }
        private async Task WithdrawCompletedAlert(WithdrawModel transaction)
        {
            var user = await _unitOfWork.UserRepository.GetUser(transaction.UserId);

            var message = await _botMenu.Wallet.WithdrawCompletedAlert(user!, transaction, false);
            await _unitOfWork.UserRepository.UpdateUserMessageId(user!.UserId, message!.MessageId);
            await _unitOfWork.SaveChanges();
        }
        public async Task WithdrawValidate()
        {
            var verifyTransactions = await WithdrawValidateOrder();

            foreach (var transaction in verifyTransactions)
            {
                await _unitOfWork.WithdrawOrderRepository.UpdateWithdrawOrderStatus(transaction.OrderId, transaction.Status);
                await _unitOfWork.WithdrawRepository.AddWithdraw(transaction);
                await _unitOfWork.SaveChanges();

                _logger.LogInformation("Coin sending confirmation received {0}", JsonConvert.SerializeObject(transaction, Formatting.Indented));

                await WithdrawCompletedAlert(transaction);
            }
        }
        public async Task<List<WithdrawModel>> WithdrawValidateOrder()
        {
            var withdrawHistory = await _binanceService.GetWithdrawHistory();
            var withdrawOrders = await _unitOfWork.WithdrawOrderRepository.GetWithdrawOrderList();
            if (!withdrawHistory.Status || withdrawHistory.Data == null)
            {
                throw new Exception("Retrieving output history is not successful");
            }
            var withdrawOrdersFilt = withdrawOrders.Where(x => x.Status != WithdrawalStatus.Completed);
            var withdrawHistoryFilt = withdrawHistory.Data.Where(x => x.Status == WithdrawalStatus.Completed);

            var verifyTransactions = new List<WithdrawModel>();

            foreach (var order in withdrawOrdersFilt)
            {
                var withdraw = withdrawHistoryFilt.SingleOrDefault(x => x.Id == order.IdOrder);
                if (withdraw is not null)
                {
                    var withdrawModel = new WithdrawModel()
                    {
                        AddressFrom = "Default.Binance",
                        AddressTo = order.AddressTo,
                        Network = order.Network,
                        Asset = order.Asset,
                        Fee = order.Fee,
                        UserId = order.UserId,
                        Status = withdraw.Status,
                        Amount = order.Amount,
                        Hash = withdraw.TransactionId,
                        OrderId = order.IdOrder,
                        ChargeToCapital = order.ChargeToCapital
                        
                    };
                    verifyTransactions.Add(withdrawModel);
                }
            }

            return verifyTransactions;
        }
        #endregion

        #region Deposit
        private async Task DepositReception(DepositModel deposit)
        {
            var userModel = await _unitOfWork.UserRepository.GetUser(deposit.AddressTo, deposit.Network);

            await _unitOfWork.DepositRepository.AddDeposite(deposit);
            await _unitOfWork.BalanceRepository.AddToBalance(deposit.UserId, deposit.Amount, deposit.Asset);
            await _unitOfWork.SaveChanges();

            _logger.LogInformation("The user has received a deposit - {@Deposit}", new { deposit.Hash });
            _logger.LogDebug("The user has received a deposit - {@Deposit}", JsonConvert.SerializeObject(deposit, Formatting.Indented));

            await DepositUserAlert(userModel!, deposit);
        }
        private async Task DepositUserAlert(UserModel userModel, DepositModel depositModel)
        {
            var message = await _botMenu.Wallet.DepositAlert(userModel, depositModel, false);

            await _unitOfWork.UserRepository.UpdateUserMessageId(userModel.UserId, message!.MessageId);
            await _unitOfWork.SaveChanges();
        }
        public async Task DepositScanTransaction()
        {
            var depositList = await _unitOfWork.DepositRepository.GetDepositList();
            HashSet<string> depositListHash = depositList.Select(obj => obj.Hash).ToHashSet();

            var chaines = _appConfig.AssetConfiguration.NetworkList;

            for (int i = 0; i < chaines.Count; i++)
            {
                LogContext.PushProperty("Network", chaines[i]);
                var addressList = await _unitOfWork.BlockChainWalletRepository.GetUsersAddressList(chaines[i]);
                HashSet<string> addressListHash = addressList.ToHashSet();

                foreach (var Asset in _appConfig.AssetConfiguration.CoinListByNetwork[chaines[i]])
                {
                    try
                    {
                        LogContext.PushProperty("Asset", Asset);
                        var depositeModel = await _blockChainScannerFacade.Scanners[chaines[i]].ScanUserDeposit(addressListHash, depositListHash, Asset);

                        await DepositProcessing(depositeModel, Asset, chaines[i]);
                    }
                    catch(Exception ex)
                    {
                        _logger.LogError(ex, "An error occurred while scanning and/or processing the deposit");
                    }
                }
            }
        }

        private async Task DepositProcessing(List<TransactionDTO> transactions, string asset, string network)
        {
            var depositSplit = DepositSplit(transactions);

            foreach (var deposits in depositSplit)
            {
                try
                {
                    _logger.LogInformation("Deposit processing has started - {@Count}", new { deposits.Count });

                    var user = await _unitOfWork.UserRepository.GetUser(deposits.First().To, network);
                    var amount = deposits.Sum(x => x.Value);

                    LogContext.PushProperty("UserId", user!.UserId);
                    LogContext.PushProperty("Amount", amount);

                    var transferToHot = await _blockChainTransferFacade.Transfers[network].TransferCoinToHot(user!, asset);
                    await _unitOfWork.TransferToHotRepository.AddTransferToHot(transferToHot);
                    await _unitOfWork.SaveChanges();

                    var feeForOneDeposit = (transferToHot.Fee / deposits.Count).AmountRound();
                    decimal feeInDepositCoin = deposits.First().isToken ? await DepositFeeInCoin(feeForOneDeposit, asset, network) : feeForOneDeposit;

                    foreach (var deposit in deposits)
                    {
                        var depositModel = DepositCreateModel(user!, deposit, feeInDepositCoin, asset, network);
                        await DepositReception(depositModel);
                    }

                    _logger.LogInformation("Deposit processing completed - {@Count}", new { deposits.Count });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while processing deposits - {@Deposits} ", JsonConvert.SerializeObject(deposits.Select(x => x.TransactionHash)));
                }
            }
        }

        private List<List<TransactionDTO>> DepositSplit(List<TransactionDTO> sourceList)
        {
            Dictionary<string, List<TransactionDTO>> propertyListsDictionary = new Dictionary<string, List<TransactionDTO>>();

            foreach (var item in sourceList)
            {
                string propertyValue = item.To;

                if (!propertyListsDictionary.ContainsKey(propertyValue))
                {
                    propertyListsDictionary[propertyValue] = new List<TransactionDTO>();
                }

                propertyListsDictionary[propertyValue].Add(item);
            }

            return propertyListsDictionary.Values.ToList();
        }

        private async Task<decimal> DepositFeeInCoin(decimal fee, string Asset, string network)
        {
            var mainAssetByNetwork = _appConfig.AssetConfiguration.MainAssetByNetwork[network];
            var mainCoinPriceUSD = await _infoService.GetCoinPriceUSDT(mainAssetByNetwork);
            var depositCoinPriceUSD = await _infoService.GetCoinPriceUSDT(Asset);

            return CalculationTool.ConvertCoin(fee, mainCoinPriceUSD, depositCoinPriceUSD);
        }

        private DepositModel DepositCreateModel(UserModel user, TransactionDTO transaction, decimal fee, string Asset, string network)
        {
            decimal amount = (transaction.Value - fee).AmountRound();

            return new DepositModel()
            {
                AddressFrom = transaction.From,
                AddressTo = transaction.To,
                Amount = amount,
                Asset = Asset,
                Fee = fee,
                Hash = transaction.TransactionHash,
                Network = network,
                UserId = user.UserId,
                IsInside = false
                
            };
        }
        #endregion
    }
}
