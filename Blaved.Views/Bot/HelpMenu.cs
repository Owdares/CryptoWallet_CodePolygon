using Blaved.Interfaces.Services;
using Blaved.Interfaces.Views;
using Blaved.Models;
using Blaved.Objects;
using Blaved.Objects.Models.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Blaved.Views.Bot
{
    public class HelpMenu : BotMenuBase, IHelpMenu
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IInterfaceTranslatorService _interfaceTranslatorService;
        private readonly AppConfig _appConfig;
        public HelpMenu(ITelegramBotClient botClient, IInterfaceTranslatorService interfaceTranslatorService,
            IOptions<AppConfig> appConfig, ILogger<HelpMenu> logger) : base(botClient, logger)
        {
            _botClient = botClient;
            _interfaceTranslatorService = interfaceTranslatorService;
            _appConfig = appConfig.Value;
        }

        public async Task<Message?> HelpPage(UserModel user, CancellationToken cancellationToken, bool isEdit = true)
        {
            var menuText = _interfaceTranslatorService.GetTranslation("M.Help", user.Language);
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
                    InlineKeyboardButton.WithCallbackData(text: buttonBackText,callbackData: CallbackRequestRoute.Main)
                },

            });

            return await SendMessageAsync(user, menuText, inlineKeyboard, isEdit, cancellationToken);
        }
    }
}
