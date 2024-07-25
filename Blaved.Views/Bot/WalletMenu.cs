using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
using Blaved.Models;
using Blaved.Interfaces.Services;
using Bleved.Interfaces.Services;
using Blaved.Interfaces.Views;
using Blaved.Objects.Models.Configurations;
using Microsoft.Extensions.Options;
using Blaved.Models.Info;
using Blaved.Objects;
using Blaved.Utility;
using Blaved.Objects.Models;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;

namespace Blaved.Views.Bot
{
    public class WalletMenu : BotMenuBase, IWalletMenu
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IInfoService _infoService;
        private readonly IInterfaceTranslatorService _interfaceTranslatorService;
        private readonly AppConfig _appConfig;
        public WalletMenu(ITelegramBotClient botClient, IInfoService infoService, IInterfaceTranslatorService interfaceTranslatorService,
            IOptions<AppConfig> appConfig, ILogger<WalletMenu> logger) : base(botClient, logger)
        {
            _botClient = botClient;
            _infoService = infoService;
            _interfaceTranslatorService = interfaceTranslatorService;
            _appConfig = appConfig.Value;
        }

        public async Task<Message?> Wallet(UserModel user, CancellationToken cancellationToken, int currentPage = 1, bool isEdit = true)
        {
            var menuText = _interfaceTranslatorService.GetTranslation("M.WalletTop", user.Language);
            var menuLowerText = _interfaceTranslatorService.GetTranslation("M.WalletLower", user.Language);
            var walletOneAssetMenuText = _interfaceTranslatorService.GetTranslation("M.WalletOneAsset", user.Language);
            var buttonDepositText = _interfaceTranslatorService.GetTranslation("B.Deposit", user.Language);
            var buttonWithdrawText = _interfaceTranslatorService.GetTranslation("B.Withdraw", user.Language);
            var buttonWalletTransactionHistoryText = _interfaceTranslatorService.GetTranslation("B.WalletTransactionHistory", user.Language);
            var buttonNextPageText = _interfaceTranslatorService.GetTranslation("B.NextPage", user.Language);
            var buttonBackPageText = _interfaceTranslatorService.GetTranslation("B.BackPage", user.Language);
            var buttonBackText = _interfaceTranslatorService.GetTranslation("B.BackMainMenu", user.Language);
            var buttonBonusBalanceText = _interfaceTranslatorService.GetTranslation("B.BonusBalance", user.Language);

            var coinList = _appConfig.AssetConfiguration.CoinList;
            int buttonsPerPage = 4;
            int startIndex = (currentPage - 1) * buttonsPerPage;
            int endIndex = Math.Min(startIndex + buttonsPerPage, coinList.Count);

            List<InlineKeyboardButton[]> rows = new List<InlineKeyboardButton[]>();
            if (currentPage > 1 || endIndex < coinList.Count)
            {
                var pageNavigationRow = new List<InlineKeyboardButton>();
                if (currentPage > 1)
                {
                    pageNavigationRow.Add(InlineKeyboardButton.WithCallbackData(buttonBackPageText, $"{CallbackRequestRoute.WalletPutPage}:{currentPage - 1}"));
                }
                if (endIndex < coinList.Count)
                {
                    pageNavigationRow.Add(InlineKeyboardButton.WithCallbackData(buttonNextPageText, $"{CallbackRequestRoute.WalletPutPage}:{currentPage + 1}"));
                }
                rows.Add(pageNavigationRow.ToArray());
            }
            rows.Add(new[]
            {
               InlineKeyboardButton.WithCallbackData(text: buttonBonusBalanceText, callbackData:$"{CallbackRequestRoute.BonusBalancePutPage}:1"),
            });

            rows.Add(new[]
            {
               InlineKeyboardButton.WithCallbackData(text: buttonDepositText, callbackData: $"{CallbackRequestRoute.DepositWaitAssetPutPage}:1"),
               InlineKeyboardButton.WithCallbackData(text: buttonWithdrawText, callbackData: $"{CallbackRequestRoute.WithdrawCreateWaitAssetPutPage}:1")
            });

            rows.Add(new[]
            {
               InlineKeyboardButton.WithCallbackData(text: buttonWalletTransactionHistoryText, callbackData: CallbackRequestRoute.WalletTransactionHistory),
            });

            rows.Add(new[]
            {
               InlineKeyboardButton.WithCallbackData(text: buttonBackText, callbackData: CallbackRequestRoute.Main)
            });

            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(rows);

            decimal fullBalanceUsd = 0;
            foreach (var coin in coinList)
            {
                var coinPrice = await _infoService.GetCoinPriceUSDT(coin);

                var balance = user.BalanceModel.GetBalance(coin);
                if(balance > 0)
                {
                    fullBalanceUsd += balance.AmountToUSD(coinPrice);
                }
                var bonusBalanceModel = user.BonusBalanceModel.GetBalance(coin);
                if (bonusBalanceModel > 0)
                {
                    fullBalanceUsd += bonusBalanceModel.AmountToUSD(coinPrice);
                }
            }

            menuText = string.Format(menuText, user.UserId, fullBalanceUsd.AmountCutUSD());

            for (var i = startIndex; i < endIndex; i++)
            {
                var balanace = user.BalanceModel.GetBalance(coinList[i]);
                var additionalBalance = user.BonusBalanceModel.GetBalance(coinList[i]);
                var coinPrice = await _infoService.GetCoinPriceUSDT(coinList[i]);
                var coinUrl = _appConfig.AssetConfiguration.CoinUrl[coinList[i]];
                var AssetView = _appConfig.AssetConfiguration.CoinViewName[coinList[i]];

                menuText += string.Format(walletOneAssetMenuText, 
                    AssetView, coinUrl,
                    balanace.AmountCut(), coinList[i], balanace.AmountToUSD(coinPrice).AmountCutUSD(),
                    additionalBalance.AmountCut(), coinList[i], additionalBalance.AmountToUSD(coinPrice).AmountCutUSD());

            }

            return await SendMessageAsync(user, menuText, inlineKeyboard, isEdit, cancellationToken);
        }

        #region Bonus Balance

        public async Task BonusBalanceToMainBalanceCompleteAnswer(UserModel user, string callbackQueryId, CancellationToken cancellationToken)
        {
            var menuText = _interfaceTranslatorService.GetTranslation("M.BonusBalanceToBalanceCompletedAnswer", user.Language);

            await SendMessageAnswerAsync(menuText, callbackQueryId, cancellationToken);
        }
        public async Task BonusBalanceToMainBalanceErrorAnswer(UserModel user, string callbackQueryId, CancellationToken cancellationToken)
        {
            var menuText = _interfaceTranslatorService.GetTranslation("M.BonusBalanceToBalanceErrorAnswer", user.Language);

            await SendMessageAnswerAsync(menuText, callbackQueryId, cancellationToken);
        }
        public async Task<Message?> BonusBalance(UserModel user, CancellationToken cancellationToken, int currentPage = 1, bool isEdit = true)
        {
            var menuText = _interfaceTranslatorService.GetTranslation("M.BonusBalance", user.Language);
            var bonusBalanceOneAssetPageText = _interfaceTranslatorService.GetTranslation("M.BonusBalanceOneAsset", user.Language);
            var buttonSendReferalToMainBalanceText = _interfaceTranslatorService.GetTranslation("B.BonusBalanceToBalance", user.Language);
            var buttonNextPageText = _interfaceTranslatorService.GetTranslation("B.NextPage", user.Language);
            var buttonBackPageText = _interfaceTranslatorService.GetTranslation("B.BackPage", user.Language);
            var buttonBackText = _interfaceTranslatorService.GetTranslation("B.Back", user.Language);

            var coinList = _appConfig.AssetConfiguration.CoinList;
            int buttonsPerPage = 5;
            int startIndex = (currentPage - 1) * buttonsPerPage;
            int endIndex = Math.Min(startIndex + buttonsPerPage, coinList.Count);

            List<InlineKeyboardButton[]> rows = new List<InlineKeyboardButton[]>();
            if (currentPage > 1 || endIndex < coinList.Count)
            {
                var pageNavigationRow = new List<InlineKeyboardButton>();
                if (currentPage > 1)
                {
                    pageNavigationRow.Add(InlineKeyboardButton.WithCallbackData(buttonBackPageText, $"{CallbackRequestRoute.BonusBalancePutPage}:{currentPage - 1}"));
                }
                if (endIndex < coinList.Count)
                {
                    pageNavigationRow.Add(InlineKeyboardButton.WithCallbackData(buttonNextPageText, $"{CallbackRequestRoute.BonusBalancePutPage}:{currentPage + 1}"));
                }
                rows.Add(pageNavigationRow.ToArray());
            }
            rows.Add(new[]
            {
               InlineKeyboardButton.WithCallbackData(text: buttonSendReferalToMainBalanceText, callbackData: CallbackRequestRoute.BonusBalanceToBalance)
            });

            rows.Add(new[]
            {
               InlineKeyboardButton.WithCallbackData(text: buttonBackText, callbackData: $"{CallbackRequestRoute.WalletPutPage}:1")
            });

            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(rows);

            decimal fullBonusBalanceUsd = 0;
            foreach (var coin in coinList)
            {
                var balance = user.BonusBalanceModel.GetBalance(coin);
                if (balance > 0)
                {
                    var coinPrice = await _infoService.GetCoinPriceUSDT(coin);
                    fullBonusBalanceUsd += balance.AmountToUSD(coinPrice);
                }
            }

            menuText = string.Format(menuText, fullBonusBalanceUsd.AmountCutUSD());

            for (var i = startIndex; i < endIndex; i++)
            {
                var additionalBalance = user.BonusBalanceModel.GetBalance(coinList[i]);
                var coinPrice = await _infoService.GetCoinPriceUSDT(coinList[i]);
                var coinUrl = _appConfig.AssetConfiguration.CoinUrl[coinList[i]];

                var AssetView = _appConfig.AssetConfiguration.CoinViewName[coinList[i]];

                menuText += string.Format(bonusBalanceOneAssetPageText,
                   AssetView, coinUrl,
                   additionalBalance.AmountCut(), coinList[i], additionalBalance.AmountToUSD(coinPrice).AmountCutUSD());
            }

            return await SendMessageAsync(user, menuText, inlineKeyboard, isEdit, cancellationToken);
        }

        #endregion

        #region Withdraw
        public async Task<Message?> WithdrawCreateWaitNetwork(UserModel user, string Asset, CancellationToken cancellationToken, bool isEdit = true)
        {
            var menuText = _interfaceTranslatorService.GetTranslation("M.WithdrawCreateWaitNetwork", user.Language);
            var buttonBackText = _interfaceTranslatorService.GetTranslation("B.BackUpdateAsset", user.Language);

            var listChaine = _appConfig.AssetConfiguration.NetworkListByCoin[Asset];
            var chaineViewName = _appConfig.AssetConfiguration.NetworkViewName;

            var assetUrl = _appConfig.AssetConfiguration.CoinUrl[Asset];

            menuText = string.Format(menuText,
                Asset, assetUrl);

            var buttons = listChaine.Select(network => InlineKeyboardButton.WithCallbackData(chaineViewName[network], $"{CallbackRequestRoute.WithdrawCreateWaitAddressPutAssetNetwork}:{Asset}_{network}")).ToArray();

            int buttonsPerRow = 1;

            var buttonRows = buttons.Select((button, index) => new { Button = button, Index = index })
                .GroupBy(x => x.Index / buttonsPerRow)
                .Select(group => group.Select(x => x.Button).ToArray())
                .ToList();

            buttonRows.Add(new[] { InlineKeyboardButton.WithCallbackData(buttonBackText, $"{CallbackRequestRoute.WithdrawCreateWaitAssetPutPage}:1") });

            var inlineKeyboard = new InlineKeyboardMarkup(buttonRows);

            return await SendMessageAsync(user, menuText, inlineKeyboard, isEdit, cancellationToken);
        }
        public async Task<Message?> WithdrawCreateWaitAsset(UserModel user, CancellationToken cancellationToken, int currentPage = 1, bool isEdit = true)
        {
            var menuText = _interfaceTranslatorService.GetTranslation("M.WithdrawCreateWaitAsset", user.Language);
            var buttonNextPageText = _interfaceTranslatorService.GetTranslation("B.NextPage", user.Language);
            var buttonBackPageText = _interfaceTranslatorService.GetTranslation("B.BackPage", user.Language);
            var buttonBackText = _interfaceTranslatorService.GetTranslation("B.Back", user.Language);

            var AssetList = _appConfig.AssetConfiguration.CoinList;

            var buttons = AssetList.Select(name => InlineKeyboardButton.WithCallbackData(text: name, callbackData: $"{CallbackRequestRoute.WithdrawCreateWaitNetworkPutAsset}:{name}")).ToArray();

            int buttonsPerRow = 3;
            int buttonsPerColum = 4;

            int buttonsPerPage = buttonsPerRow * buttonsPerColum;

            int startIndex = (currentPage - 1) * buttonsPerPage;
            int endIndex = Math.Min(startIndex + buttonsPerPage, AssetList.Count);

            var buttonRows = buttons
                .Skip(startIndex)
                .Take(buttonsPerPage)
                .Select((button, index) => new { Button = button, Index = index, RowIndex = index / buttonsPerRow, ColumnIndex = index % buttonsPerRow })
                .GroupBy(x => x.RowIndex)
                .Take(buttonsPerColum)
                .Select(group => group.OrderBy(x => x.ColumnIndex).Select(x => x.Button).ToArray())
                .ToList();

            if (currentPage > 1 || endIndex < AssetList.Count)
            {
                var pageNavigationRow = new List<InlineKeyboardButton>();
                if (currentPage > 1)
                {
                    pageNavigationRow.Add(InlineKeyboardButton.WithCallbackData(buttonBackPageText, $"{CallbackRequestRoute.WithdrawCreateWaitAssetPutPage}:{currentPage - 1}"));
                }
                if (endIndex < AssetList.Count)
                {
                    pageNavigationRow.Add(InlineKeyboardButton.WithCallbackData(buttonNextPageText, $"{CallbackRequestRoute.WithdrawCreateWaitAssetPutPage}:{currentPage + 1}"));
                }
                buttonRows.Add(pageNavigationRow.ToArray());
            }

            buttonRows.Add(new[] { InlineKeyboardButton.WithCallbackData(text: buttonBackText, callbackData: $"{CallbackRequestRoute.WalletPutPage}:1") });

            var inlineKeyboard = new InlineKeyboardMarkup(buttonRows);

            return await SendMessageAsync(user, menuText, inlineKeyboard, isEdit, cancellationToken);
        }
        public async Task<Message?> WithdrawCreateWaitAddress(UserModel user, string Asset, string network, CancellationToken cancellationToken, bool isEdit = true)
        {
            var menuText = _interfaceTranslatorService.GetTranslation("M.WithdrawCreateWaitAddress", user.Language);
            var buttonBackText = _interfaceTranslatorService.GetTranslation("B.BackUpdateNetwork", user.Language);

            var networkViewName = _appConfig.AssetConfiguration.NetworkViewName[network];

            menuText = string.Format(menuText,
                Asset, networkViewName.Replace("(", "\\(")).Replace(")", "\\)");

            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: buttonBackText, callbackData:$"{CallbackRequestRoute.WithdrawCreateWaitNetworkPutAsset}:{Asset}"),
                },
            });

            return await SendMessageAsync(user, menuText, inlineKeyboard, isEdit, cancellationToken);
        }
        public async Task<Message?> WithdrawCreateWaitAmount(UserModel user, CancellationToken cancellationToken, bool isEdit = true)
        {
            var menuText = _interfaceTranslatorService.GetTranslation("M.WithdrawCreateWaitAmount", user.Language);
            var buttonBackText = _interfaceTranslatorService.GetTranslation("B.BackUpdateAddress", user.Language);
            var buttonMinAmountText = _interfaceTranslatorService.GetTranslation("B.MinAmount", user.Language);
            var buttonMaxAmountText = _interfaceTranslatorService.GetTranslation("B.MaxAmount", user.Language);

            string Asset = user.MessagesWithdrawModel.Asset;
            string network = user.MessagesWithdrawModel.Network;

            var coinInfo = await _infoService.GetInfoForWithdraw(Asset, network);
            var coinPriceUSDT = await _infoService.GetCoinPriceUSDT(Asset);
            var coinBalance = user.BalanceModel.GetBalance(Asset);

            decimal minAmount = coinInfo.MinimalSum;
            decimal maxAmount = coinBalance - coinInfo.WithdrawCombineFee;
            var assetUrl = _appConfig.AssetConfiguration.CoinUrl[Asset];

            menuText = string.Format(menuText,
                Asset, assetUrl,
                maxAmount.AmountCut(), Asset, maxAmount.AmountToUSD(coinPriceUSDT).AmountCutUSD(),
                minAmount.AmountCut(), Asset, minAmount.AmountToUSD(coinPriceUSDT).AmountCutUSD(),
                coinInfo.WithdrawCombineFee.AmountCut(), Asset, coinInfo.WithdrawCombineFee.AmountToUSD(coinPriceUSDT).AmountCutUSD(),
                coinBalance.AmountCut(), Asset, coinBalance.AmountToUSD(coinPriceUSDT).AmountCutUSD());

            buttonMinAmountText = string.Format(buttonMinAmountText, minAmount.AmountCut(), minAmount.AmountToUSD(coinPriceUSDT).AmountCutUSD());
            buttonMaxAmountText = string.Format(buttonMaxAmountText, maxAmount.AmountCut(), maxAmount.AmountToUSD(coinPriceUSDT).AmountCutUSD());

            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: buttonMinAmountText, callbackData: $"{CallbackRequestRoute.WithdrawCreatePutAmount}:{minAmount}"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: buttonMaxAmountText, callbackData: $"{CallbackRequestRoute.WithdrawCreatePutAmount}:{maxAmount}"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: buttonBackText, callbackData:$"{CallbackRequestRoute.WithdrawCreateWaitAddressPutAssetNetwork}:{Asset}_{network}"),
                },
            });

            return await SendMessageAsync(user, menuText, inlineKeyboard, isEdit, cancellationToken);


        }
        public async Task<Message?> WithdrawConfirm(UserModel user, CancellationToken cancellationToken, bool isEdit = true)
        {
            var menuText = _interfaceTranslatorService.GetTranslation("M.WithdrawConfirm", user.Language);
            var buttonConfirmText = _interfaceTranslatorService.GetTranslation("B.Confirm", user.Language);
            var buttonCancelText = _interfaceTranslatorService.GetTranslation("B.Cancel", user.Language);
            var buttonBackText = _interfaceTranslatorService.GetTranslation("B.BackUpdateAmount", user.Language);

            string Asset = user.MessagesWithdrawModel.Asset;
            string network = user.MessagesWithdrawModel.Network;
            decimal amount = user.MessagesWithdrawModel.Amount;
            string address = user.MessagesWithdrawModel.Address;
            var networkViewName = _appConfig.AssetConfiguration.NetworkViewName[network];
            decimal coinPrice = await _infoService.GetCoinPriceUSDT(Asset);
            var networkUrl = _appConfig.AssetConfiguration.NetworkUrl[network];
            var assetUrl = _appConfig.AssetConfiguration.CoinUrl[Asset];
            var coinInfo = await _infoService.GetInfoForWithdraw(Asset, network);
            decimal completeSum = (coinInfo.WithdrawCombineFee + amount).AmountRound();

            var withdrawMoreInfoUrl = _appConfig.UrlConfiguration.MediaUrl.WithdrawMoreInfoByLanguage[user.Language];

            menuText = string.Format(menuText,
                Asset, assetUrl,
                // {0} Network
                networkViewName.Replace("(", "\\(").Replace(")", "\\)"), networkUrl,
                // {1} Address
                address,
                // {2} Amount - {3} Asset - {4} SumToUSD
                amount.AmountCut(), Asset, amount.AmountToUSD(coinPrice).AmountCutUSD(),
                // {5} WithdrawFee - {6} Asset - {7} WithdrawFeeToUSD
                coinInfo.WithdrawCombineFee.AmountCut(), Asset, coinInfo.WithdrawCombineFee.AmountToUSD(coinPrice).AmountCutUSD(),
                // {8} CompleteSum - {9} Asset - {10} CompleteSumToUSD
                completeSum.AmountCut(), Asset, completeSum.AmountToUSD(coinPrice).AmountCutUSD(),
                withdrawMoreInfoUrl);

            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                 new[]
                 {
                     InlineKeyboardButton.WithCallbackData(text: buttonConfirmText, callbackData: CallbackRequestRoute.WithdrawConfirm),
                     InlineKeyboardButton.WithCallbackData(text: buttonCancelText, callbackData: $"{CallbackRequestRoute.WalletPutPage}:1"),
                 },
                 new[]
                 {
                     InlineKeyboardButton.WithCallbackData(text: buttonBackText, callbackData: CallbackRequestRoute.WithdrawCreateWaitAmount),
                 },
            });

            return await SendMessageAsync(user, menuText, inlineKeyboard, isEdit, cancellationToken);

        }
        public async Task<Message?> WithdrawCreateNotCorrectAddress(UserModel user, CancellationToken cancellationToken, bool isEdit = true)
        {
            var menuText = _interfaceTranslatorService.GetTranslation("M.WithdrawCreateNotCorrectAddress", user.Language);
            var buttonBackText = _interfaceTranslatorService.GetTranslation("B.BackUpdateNetwork", user.Language);

            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: buttonBackText, callbackData:$"{CallbackRequestRoute.WithdrawCreateWaitNetworkPutAsset}:{user.MessagesWithdrawModel.Asset}"),
                },
            });

            return await SendMessageAsync(user, menuText, inlineKeyboard, isEdit, cancellationToken);
        }
        public async Task<Message?> WithdrawCreateNotCorrectAmount(UserModel user, CancellationToken cancellationToken, bool isEdit = true)
        {
            var menuText = _interfaceTranslatorService.GetTranslation("M.WithdrawCreateNotCorrectAmount", user.Language);
            var buttonBackText = _interfaceTranslatorService.GetTranslation("B.BackUpdateAddress", user.Language);

            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: buttonBackText, callbackData:$"{CallbackRequestRoute.WithdrawCreateWaitAddressPutAssetNetwork}:{user.MessagesWithdrawModel.Asset}_{user.MessagesWithdrawModel.Network}"),
                },
            });

            return await SendMessageAsync(user, menuText, inlineKeyboard, isEdit, cancellationToken);
        }
        public async Task WithdrawSentAnswer(UserModel user, string callbackQueryId, CancellationToken cancellationToken)
        {
            var menuText = _interfaceTranslatorService.GetTranslation("M.WithdrawSentAnswer", user.Language);

            await SendMessageAnswerAsync(menuText, callbackQueryId, cancellationToken);
        }
        public async Task WithdrawCreateInsufficientBalanceAnswer(UserModel user, InfoForWithdrawModel coinInfo, string callbackQueryId, CancellationToken cancellationToken)
        {
            var menuText = _interfaceTranslatorService.GetTranslation("M.WithdrawCreateInsufficientBalanceAnswer", user.Language);

            string Asset = coinInfo.Asset;
            decimal withdrawFee = coinInfo.WithdrawCombineFee;

            decimal coinBalance = user.BalanceModel.GetBalance(Asset);
            decimal coinPriceUSDT = await _infoService.GetCoinPriceUSDT(Asset);
            decimal minSum = coinInfo.MinimalSum;

            menuText = string.Format(menuText,
                minSum.AmountCut(), Asset, minSum.AmountToUSD(coinPriceUSDT).AmountCutUSD(),
                withdrawFee.AmountCut(), Asset, withdrawFee.AmountToUSD(coinPriceUSDT).AmountCutUSD(),
                coinBalance.AmountCut(), Asset, coinBalance.AmountToUSD(coinPriceUSDT).AmountCutUSD());

            await SendMessageAnswerAsync(menuText, callbackQueryId, cancellationToken);
        }
        public async Task<Message?> WithdrawErrorAlert(UserModel user, CancellationToken cancellationToken, bool isEdit = true)
        {
            var menuText = _interfaceTranslatorService.GetTranslation("M.WithdrawErrorAlert", user.Language);
            var buttonHelpUrlText = _interfaceTranslatorService.GetTranslation("B.HelpUrl", user.Language);
            var buttonBackText = _interfaceTranslatorService.GetTranslation("B.BackMainMenu", user.Language);

            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                     InlineKeyboardButton.WithUrl(text: buttonHelpUrlText, url: _appConfig.UrlConfiguration.MediaUrl.HelpByLanguage[user.Language]),
                },
                new[]
                {
                     InlineKeyboardButton.WithCallbackData(text: buttonBackText, callbackData: $"{CallbackRequestRoute.WalletPutPage}:1"),
                },
            });

            return await SendMessageAsync(user, menuText, inlineKeyboard, isEdit, cancellationToken);
        }
        public async Task<Message?> WithdrawCompletedAlert(UserModel user, WithdrawModel withdrawModel, bool isEdit = true)
        {
            var menuText = _interfaceTranslatorService.GetTranslation("M.WithdrawCompletedAlert", user.Language);
            var buttonBackText = _interfaceTranslatorService.GetTranslation("B.BackMainMenu", user.Language);

            decimal coinPrice = await _infoService.GetCoinPriceUSDT(withdrawModel.Asset);
            decimal allAmount = (withdrawModel.Amount + withdrawModel.Fee).AmountRound();
            var networkViewName = _appConfig.AssetConfiguration.NetworkViewName[withdrawModel.Network];

            var assetUrl = _appConfig.AssetConfiguration.CoinUrl[withdrawModel.Asset];
            var networkUrl = _appConfig.AssetConfiguration.NetworkUrl[withdrawModel.Network];

            string scanHashUrl = _appConfig.AssetConfiguration.NetworkScanHashUrl[withdrawModel.Network] + withdrawModel.Hash;

            menuText = string.Format(menuText,
                withdrawModel.Asset, assetUrl,
                networkViewName.Replace("(", "\\(").Replace(")", "\\)"), networkUrl,
                withdrawModel.AddressTo,
                withdrawModel.Amount.AmountCut(), withdrawModel.Asset, withdrawModel.Amount.AmountToUSD(coinPrice).AmountCutUSD(),
                withdrawModel.Fee.AmountCut(), withdrawModel.Asset, withdrawModel.Fee.AmountToUSD(coinPrice).AmountCutUSD(),
                allAmount.AmountCut(), withdrawModel.Asset, allAmount.AmountToUSD(coinPrice).AmountCutUSD(),
                withdrawModel.Hash.HashCut(), scanHashUrl);


            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                     InlineKeyboardButton.WithCallbackData(text: buttonBackText, callbackData:$"{CallbackRequestRoute.WalletPutPage}:1"),
                },
            });

            return await SendMessageAsync(user, menuText, inlineKeyboard, isEdit);
        }
        #endregion

        #region Deposit
        public async Task<Message?> DepositWaitAsset(UserModel user, CancellationToken cancellationToken, int currentPage = 1, bool isEdit = true)
        {
            var menuText = _interfaceTranslatorService.GetTranslation("M.DepositWaitAsset", user.Language);
            var buttonNextPageText = _interfaceTranslatorService.GetTranslation("B.NextPage", user.Language);
            var buttonBackPageText = _interfaceTranslatorService.GetTranslation("B.BackPage", user.Language);
            var buttonBackText = _interfaceTranslatorService.GetTranslation("B.Back", user.Language);

            var AssetList = _appConfig.AssetConfiguration.CoinList;

            var buttons = AssetList.Select(name => InlineKeyboardButton.WithCallbackData(text: name, callbackData: $"{CallbackRequestRoute.DepositWaitNetworkPutAsset}:{name}")).ToArray();

            int buttonsPerRow = 3; 
            int buttonsPerColum = 4;

            int buttonsPerPage = buttonsPerRow * buttonsPerColum;

            int startIndex = (currentPage - 1) * buttonsPerPage;
            int endIndex = Math.Min(startIndex + buttonsPerPage, AssetList.Count);

            var buttonRows = buttons
                .Skip(startIndex)
                .Take(buttonsPerPage)
                .Select((button, index) => new { Button = button, Index = index, RowIndex = index / buttonsPerRow, ColumnIndex = index % buttonsPerRow })
                .GroupBy(x => x.RowIndex)
                .Take(buttonsPerColum)
                .Select(group => group.OrderBy(x => x.ColumnIndex).Select(x => x.Button).ToArray())
                .ToList();

            if (currentPage > 1 || endIndex < AssetList.Count)
            {
                var pageNavigationRow = new List<InlineKeyboardButton>();
                if (currentPage > 1)
                {
                    pageNavigationRow.Add(InlineKeyboardButton.WithCallbackData(buttonBackPageText, $"{CallbackRequestRoute.DepositWaitAssetPutPage}:{currentPage - 1}"));
                }
                if (endIndex < AssetList.Count)
                {
                    pageNavigationRow.Add(InlineKeyboardButton.WithCallbackData(buttonNextPageText, $"{CallbackRequestRoute.DepositWaitAssetPutPage}:{currentPage + 1}"));
                }
                buttonRows.Add(pageNavigationRow.ToArray());
            }

            buttonRows.Add(new[] { InlineKeyboardButton.WithCallbackData(text: buttonBackText, callbackData: $"{CallbackRequestRoute.WalletPutPage}:1") });

            var inlineKeyboard = new InlineKeyboardMarkup(buttonRows);

            return await SendMessageAsync(user, menuText, inlineKeyboard, isEdit, cancellationToken);
        }
        public async Task<Message?> DepositWaitNetwork(UserModel user, string Asset, CancellationToken cancellationToken, bool isEdit = true)
        {
            var menuText = _interfaceTranslatorService.GetTranslation("M.DepositWaitNetwork", user.Language);
            var buttonBackText = _interfaceTranslatorService.GetTranslation("B.BackUpdateAsset", user.Language);

            var listChaine = _appConfig.AssetConfiguration.NetworkListByCoin[Asset];
            var chaineViewName = _appConfig.AssetConfiguration.NetworkViewName;

            var assetUrl = _appConfig.AssetConfiguration.CoinUrl[Asset];

            menuText = string.Format(menuText,
                Asset, assetUrl);

            var buttons = listChaine.Select(network => InlineKeyboardButton.WithCallbackData(chaineViewName[network], $"{CallbackRequestRoute.DepositViewAddressPutAssetNetwork}:{Asset}_{network}")).ToArray();

            int buttonsPerRow = 1;

            var buttonRows = buttons.Select((button, index) => new { Button = button, Index = index })
                .GroupBy(x => x.Index / buttonsPerRow)
                .Select(group => group.Select(x => x.Button).ToArray())
                .ToList();

            buttonRows.Add(new[] { InlineKeyboardButton.WithCallbackData(buttonBackText, $"{CallbackRequestRoute.DepositWaitAssetPutPage}:1") });

            var inlineKeyboard = new InlineKeyboardMarkup(buttonRows);

            return await SendMessageAsync(user, menuText, inlineKeyboard, isEdit, cancellationToken);

        }
        public async Task<Message?> DepositViewAddress(UserModel user, string Asset, string network, CancellationToken cancellationToken, bool isEdit = true)
        {
            var menuText = _interfaceTranslatorService.GetTranslation("M.DepositViewAddress", user.Language);
            var buttonBackText = _interfaceTranslatorService.GetTranslation("B.BackUpdateNetwork", user.Language);

            var infoForDeposit = await _infoService.GetInfoForDeposit(Asset, network);
            decimal assetPrice = await _infoService.GetCoinPriceUSDT(Asset);

            var assetUrl = _appConfig.AssetConfiguration.CoinUrl[Asset];
            var networkUrl = _appConfig.AssetConfiguration.NetworkUrl[network];
            var networkViewName = _appConfig.AssetConfiguration.NetworkViewName[network];

            var depositMoreInfoUrl = _appConfig.UrlConfiguration.MediaUrl.DepositMoreInfoByLanguage[user.Language];

            menuText = string.Format(menuText, 
                Asset, assetUrl,
                networkViewName.Replace("(", "\\(").Replace(")", "\\)"), networkUrl,
                infoForDeposit.MinAmount.AmountCut(), Asset, infoForDeposit.MinAmount.AmountToUSD(assetPrice).AmountCutUSD(),
                user.BlockChainWalletModel.GetAddress(network), depositMoreInfoUrl);

            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: buttonBackText, callbackData:$"{CallbackRequestRoute.DepositWaitNetworkPutAsset}:{Asset}"),
                },
            });

            return await SendMessageAsync(user, menuText, inlineKeyboard, isEdit, cancellationToken);
        }
        public async Task<Message?> DepositAlert(UserModel user, DepositModel depositeModel, bool isEdit = true)
        {
            var menuText = _interfaceTranslatorService.GetTranslation("M.DepositAlert", user.Language);
            var buttonBackText = _interfaceTranslatorService.GetTranslation("B.BackMainMenu", user.Language);

            decimal coinPrice = await _infoService.GetCoinPriceUSDT(depositeModel.Asset);

            var networkViewName = _appConfig.AssetConfiguration.NetworkViewName[depositeModel.Network];

            var allAmount = (depositeModel.Amount + depositeModel.Fee).AmountRound();
            var assetUrl = _appConfig.AssetConfiguration.CoinUrl[depositeModel.Asset];
            var networkUrl = _appConfig.AssetConfiguration.NetworkUrl[depositeModel.Network];

            menuText = string.Format(menuText,
                depositeModel.Asset, assetUrl,
                networkViewName.Replace("(", "\\(").Replace(")", "\\)"), networkUrl,
                depositeModel.AddressFrom,
                depositeModel.Amount.AmountCut(), depositeModel.Asset, depositeModel.Amount.AmountToUSD(coinPrice).AmountCutUSD(),
                depositeModel.Fee.AmountCut(), depositeModel.Asset, depositeModel.Fee.AmountToUSD(coinPrice).AmountCutUSD(),
                allAmount.AmountCut(), depositeModel.Asset, allAmount.AmountToUSD(coinPrice).AmountCutUSD());

            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                   InlineKeyboardButton.WithCallbackData(text: buttonBackText, callbackData: CallbackRequestRoute.Main),
                }
            });

            return await SendMessageAsync(user, menuText, inlineKeyboard, isEdit);

        }
        #endregion

        #region TransactionHistory

        public async Task<Message?> WalletTransactionHistory(UserModel user, CancellationToken cancellationToken, bool isEdit = true)
        {
            var menuText = _interfaceTranslatorService.GetTranslation("M.WalletTransactionHistory", user.Language);
            var buttonDepositeHistoryText = _interfaceTranslatorService.GetTranslation("B.DepositHistory", user.Language);
            var buttonWithdrawHistoryText = _interfaceTranslatorService.GetTranslation("B.WithdrawHistory", user.Language);
            var buttonBackText = _interfaceTranslatorService.GetTranslation("B.Back", user.Language);

            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: buttonDepositeHistoryText, callbackData:$"{CallbackRequestRoute.DepositHistoryWaitNetwork}"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: buttonWithdrawHistoryText, callbackData:$"{CallbackRequestRoute.WithdrawHistoryWaitNetwork}"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: buttonBackText, callbackData:$"{CallbackRequestRoute.WalletPutPage}:1"),
                },
            });

            return await SendMessageAsync(user, menuText, inlineKeyboard, isEdit, cancellationToken);
        }
        public async Task<Message?> WithdrawHistoryWaitNetwork(UserModel user, CancellationToken cancellationToken, bool isEdit = true)
        {
            var menuText = _interfaceTranslatorService.GetTranslation("M.WithdrawHistoryWaitNetwork", user.Language);
            var buttonBackText = _interfaceTranslatorService.GetTranslation("B.Back", user.Language);

            var chaine = _appConfig.AssetConfiguration.NetworkViewName;

            var buttons = chaine.Select(network => InlineKeyboardButton.WithCallbackData(network.Value, $"{CallbackRequestRoute.WithdrawHistoryWaitNumPutNetworkPage}:{network.Key}_1")).ToArray();

            int buttonsPerRow = 1;

            var buttonRows = buttons.Select((button, index) => new { Button = button, Index = index })
                .GroupBy(x => x.Index / buttonsPerRow)
                .Select(group => group.Select(x => x.Button).ToArray())
                .ToList();

            buttonRows.Add(new[] { InlineKeyboardButton.WithCallbackData(buttonBackText, $"{CallbackRequestRoute.WalletTransactionHistory}") });

            var inlineKeyboard = new InlineKeyboardMarkup(buttonRows);

            return await SendMessageAsync(user, menuText, inlineKeyboard, isEdit, cancellationToken);

        }
        public async Task<Message?> DepositHistoryWaitNetwork(UserModel user, CancellationToken cancellationToken, bool isEdit = true)
        {
            var menuText = _interfaceTranslatorService.GetTranslation("M.DepositHistoryWaitNetwork", user.Language);
            var buttonBackText = _interfaceTranslatorService.GetTranslation("B.Back", user.Language);

            var chaine = _appConfig.AssetConfiguration.NetworkViewName;

            var buttons = chaine.Select(network => InlineKeyboardButton.WithCallbackData(network.Value, $"{CallbackRequestRoute.DepositHistoryWaitNumPutNetworkPage}:{network.Key}_1")).ToArray();

            int buttonsPerRow = 1;

            var buttonRows = buttons.Select((button, index) => new { Button = button, Index = index })
                .GroupBy(x => x.Index / buttonsPerRow)
                .Select(group => group.Select(x => x.Button).ToArray())
                .ToList();

            buttonRows.Add(new[] { InlineKeyboardButton.WithCallbackData(buttonBackText, $"{CallbackRequestRoute.WalletTransactionHistory}") });

            var inlineKeyboard = new InlineKeyboardMarkup(buttonRows);

            return await SendMessageAsync(user, menuText, inlineKeyboard, isEdit, cancellationToken);
        }
        public async Task<Message?> DepositHistoryList(UserModel user, string network, List<DepositModel> history,
            CancellationToken cancellationToken, int currentPage = 1, bool isEdit = true)
        {
            var menuText = _interfaceTranslatorService.GetTranslation("M.DepositHistoryList", user.Language);

            var buttonNextPageText = _interfaceTranslatorService.GetTranslation("B.NextPage", user.Language);
            var buttonBackPageText = _interfaceTranslatorService.GetTranslation("B.BackPage", user.Language);
            var buttonBackText = _interfaceTranslatorService.GetTranslation("B.BackUpdateNetwork", user.Language);

            int buttonsPerPage = 10;

            int startIndex = (currentPage - 1) * buttonsPerPage;
            int endIndex = Math.Min(startIndex + buttonsPerPage, history.Count);

            List<InlineKeyboardButton[]> buttons = (await Task.WhenAll(history
                .Skip(startIndex)
                .Take(buttonsPerPage)
                .Select(async x =>
                {
                    var assetPrice = await _infoService.GetCoinPriceUSDT(x.Asset);
                    return new[] { InlineKeyboardButton.WithCallbackData($"{x.Amount.AmountCut()} {x.Asset} (${x.Amount.AmountToUSD(assetPrice).AmountCutUSD()})", $"{CallbackRequestRoute.DepositHistoryPutNum}:{x.Id}") };
                }))).ToList();

            if (currentPage > 1 || endIndex < history.Count)
            {
                var pageNavigationRow = new List<InlineKeyboardButton>();
                if (currentPage > 1)
                {
                    pageNavigationRow.Add(InlineKeyboardButton.WithCallbackData(buttonBackPageText, $"{CallbackRequestRoute.DepositHistoryWaitNumPutNetworkPage}:{network}_{currentPage - 1}"));
                }
                if (endIndex < history.Count)
                {
                    pageNavigationRow.Add(InlineKeyboardButton.WithCallbackData(buttonNextPageText, $"{CallbackRequestRoute.DepositHistoryWaitNumPutNetworkPage}:{network}_{currentPage + 1}"));
                }
                buttons.Add(pageNavigationRow.ToArray());
            }
            
            buttons.Add(new[] { InlineKeyboardButton.WithCallbackData(buttonBackText, CallbackRequestRoute.DepositHistoryWaitNetwork) });
            

            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(buttons);

            return await SendMessageAsync(user, menuText, inlineKeyboard, isEdit, cancellationToken);
        }
        public async Task<Message?> WithdrawHistoryList(UserModel user, string network, List<WithdrawModel> history,
            CancellationToken cancellationToken, int currentPage = 1, bool isEdit = true)
        {
            var menuText = _interfaceTranslatorService.GetTranslation("M.WithdrawHistoryList", user.Language);
            var buttonNextPageText = _interfaceTranslatorService.GetTranslation("B.NextPage", user.Language);
            var buttonBackPageText = _interfaceTranslatorService.GetTranslation("B.BackPage", user.Language);
            var buttonBackText = _interfaceTranslatorService.GetTranslation("B.BackUpdateNetwork", user.Language);

            int buttonsPerPage = 10;
            int startIndex = (currentPage - 1) * buttonsPerPage;
            int endIndex = Math.Min(startIndex + buttonsPerPage, history.Count);
            
            List<InlineKeyboardButton[]> buttons = (await Task.WhenAll(history
                .Skip(startIndex)
                .Take(buttonsPerPage)
                .Select(async x =>
                {
                    var assetPrice = await _infoService.GetCoinPriceUSDT(x.Asset);
                    return new[] { InlineKeyboardButton.WithCallbackData($"{x.Amount.AmountCut()} {x.Asset} (${x.Amount.AmountToUSD(assetPrice).AmountCutUSD()})", $"{CallbackRequestRoute.WithdrawHistoryPutNum}:{x.Id}") };
                }))).ToList();

            if (currentPage > 1 || endIndex < history.Count)
            {
                var pageNavigationRow = new List<InlineKeyboardButton>();
                if (currentPage > 1)
                {
                    pageNavigationRow.Add(InlineKeyboardButton.WithCallbackData(buttonBackPageText, $"{CallbackRequestRoute.WithdrawHistoryWaitNumPutNetworkPage}:{network}_{currentPage - 1}"));
                }
                if (endIndex < history.Count)
                {
                    pageNavigationRow.Add(InlineKeyboardButton.WithCallbackData(buttonNextPageText, $"{CallbackRequestRoute.WithdrawHistoryWaitNumPutNetworkPage}:{network}_{currentPage + 1}"));
                }
                buttons.Add(pageNavigationRow.ToArray());
            }
            
            buttons.Add(new[] { InlineKeyboardButton.WithCallbackData(buttonBackText, CallbackRequestRoute.WithdrawHistoryWaitNetwork ) });
            

            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(buttons);

            return await SendMessageAsync(user, menuText, inlineKeyboard, isEdit, cancellationToken);
        }
        public async Task DepositHistoryNoneAnswer(UserModel user, string callbackQueryId, CancellationToken cancellationToken)
        {
            var menuText = _interfaceTranslatorService.GetTranslation("M.DepositHistoryNoneAnswer", user.Language);

            await SendMessageAnswerAsync(menuText, callbackQueryId, cancellationToken);
        }
        public async Task WithdrawHistoryNoneAnswer(UserModel user, string callbackQueryId, CancellationToken cancellationToken)
        {
            var menuText = _interfaceTranslatorService.GetTranslation("M.WithdrawHistoryNoneAnswer", user.Language);

            await SendMessageAnswerAsync(menuText, callbackQueryId, cancellationToken);
        }
        public async Task<Message?> DepositHistoryView(UserModel user, DepositModel transaction, CancellationToken cancellationToken, bool isEdit = true)
        {
            var menuText = _interfaceTranslatorService.GetTranslation("M.DepositHistoryView", user.Language);
            var buttonBackText = _interfaceTranslatorService.GetTranslation("B.Back", user.Language);

            var assetUrl = _appConfig.AssetConfiguration.CoinUrl[transaction.Asset];
            var networkUrl = _appConfig.AssetConfiguration.NetworkUrl[transaction.Network];

            var allAmount = transaction.Amount + transaction.Fee;
            decimal coinPrice = await _infoService.GetCoinPriceUSDT(transaction.Asset);
            var networkViewName = _appConfig.AssetConfiguration.NetworkViewName[transaction.Network];

            string scanHashUrl = _appConfig.AssetConfiguration.NetworkScanHashUrl[transaction.Network] + transaction.Hash;

            menuText = string.Format(menuText,
                transaction.Asset, assetUrl,
                networkViewName.Replace("(", "\\(").Replace(")", "\\)"), networkUrl,
                transaction.AddressFrom,
                transaction.Amount.AmountCut(), transaction.Asset, transaction.Amount.AmountToUSD(coinPrice).AmountCutUSD(),
                transaction.Fee.AmountCut(), transaction.Asset, transaction.Fee.AmountToUSD(coinPrice).AmountCutUSD(),
                allAmount.AmountCut(), transaction.Asset, allAmount.AmountToUSD(coinPrice).AmountCutUSD(),
                transaction.Hash.HashCut(), scanHashUrl,
                transaction.CreatedAt.ToShortDateString(),
                transaction.CreatedAt.ToShortTimeString());


            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: buttonBackText, callbackData:$"{CallbackRequestRoute.DepositHistoryWaitNumPutNetworkPage}:{transaction.Network}_1"),

                },
            });

            return await SendMessageAsync(user, menuText, inlineKeyboard, isEdit, cancellationToken);
        }
        public async Task<Message?> WithdrawHistoryView(UserModel user, WithdrawModel transaction, CancellationToken cancellationToken, bool isEdit = true)
        {
            var menuText = _interfaceTranslatorService.GetTranslation("M.WithdrawHistoryView", user.Language);
            var buttonBackText = _interfaceTranslatorService.GetTranslation("B.Back", user.Language);

            decimal coinPrice = await _infoService.GetCoinPriceUSDT(transaction.Asset);
            var networkViewName = _appConfig.AssetConfiguration.NetworkViewName[transaction.Network];

            var allAmount = transaction.Amount + transaction.Fee;
            var assetUrl = _appConfig.AssetConfiguration.CoinUrl[transaction.Asset];
            var networkUrl = _appConfig.AssetConfiguration.NetworkUrl[transaction.Network];

            string scanHashUrl = _appConfig.AssetConfiguration.NetworkScanHashUrl[transaction.Network] + transaction.Hash;

            menuText = string.Format(menuText,
                transaction.Asset, assetUrl,
                networkViewName.Replace("(", "\\(").Replace(")", "\\)"), networkUrl,
                transaction.AddressTo,
                transaction.Amount.AmountCut(), transaction.Asset, transaction.Amount.AmountToUSD(coinPrice).AmountCutUSD(),
                transaction.Fee.AmountCut(), transaction.Asset, transaction.Fee.AmountToUSD(coinPrice).AmountCutUSD(),
                allAmount.AmountCut(), transaction.Asset, allAmount.AmountToUSD(coinPrice).AmountCutUSD(),
                transaction.Hash.HashCut(), scanHashUrl,
                transaction.CreatedAt.ToShortDateString(),
                transaction.CreatedAt.ToShortTimeString());

            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: buttonBackText, callbackData:$"{CallbackRequestRoute.WithdrawHistoryWaitNumPutNetworkPage}:{transaction.Network}_1"),

                },
            });

            return await SendMessageAsync(user, menuText, inlineKeyboard, isEdit, cancellationToken);
        }
        #endregion
    }
}
