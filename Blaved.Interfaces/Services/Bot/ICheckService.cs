using Blaved.Models;

namespace Blaved.Interfaces.Services.Bot
{
    public interface ICheckService
    {
        Task CheckDelete(string url);
        Task<CheckModel> CheckCreate(long userId, string Asset, decimal amount, int count);
        Task CheckActivated(UserModel user, CheckModel checkModel);
    }
}
