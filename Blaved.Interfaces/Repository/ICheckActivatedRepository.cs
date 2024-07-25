using Blaved.Models;

namespace Blaved.Interfaces.Repository
{
    public interface ICheckActivatedRepository
    {
        Task AddCheckActivated(CheckActivatedModel checkActivated);
        Task<CheckActivatedModel?> GetCheckActivated(long userId, string url);
    }
}
