using Blaved.Models;
using Telegram.Bot.Types;

namespace Blaved.Interfaces.Views
{
    public interface IHelpMenu
    {
        Task<Message?> HelpPage(UserModel user, CancellationToken cancellationToken, bool isEdit = true);
    }
}
