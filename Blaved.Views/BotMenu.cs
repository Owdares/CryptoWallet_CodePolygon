using Blaved.Interfaces;
using Blaved.Interfaces.Views;

namespace Blaved.Views.Bot
{
    public class BotMenu : IBotMenu
    {
        public IExchangeMenu Exchange { get; }
        public ISettingsMenu Settings { get; }
        public IWalletMenu Wallet { get; }
        public IBlavedPayMenu BlavedPay { get; }
        public ICheckMenu Check { get; }
        public IHelpMenu Help { get; }
        public IMainMenu Main { get; }
        public BotMenu(ISettingsMenu settingMenu, IWalletMenu walletMenu, IHelpMenu helpMenu,
            IExchangeMenu exhangeMenu, IBlavedPayMenu blavedPay, IMainMenu mainMenu, ICheckMenu check)
        {
            Settings = settingMenu;
            Wallet = walletMenu;
            Exchange = exhangeMenu;
            BlavedPay = blavedPay;
            Help = helpMenu;
            Main = mainMenu;
            Check = check;
        }
    }
}
