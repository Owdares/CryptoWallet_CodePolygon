using Blaved.Interfaces.Views;

namespace Blaved.Interfaces
{
    public interface IBotMenu
    {
        public IExchangeMenu Exchange { get; }
        public ISettingsMenu Settings { get; }
        public IWalletMenu Wallet { get; }
        public IBlavedPayMenu BlavedPay { get; }
        public ICheckMenu Check { get; }
        public IHelpMenu Help { get; }
        public IMainMenu Main { get; }
    }
}
