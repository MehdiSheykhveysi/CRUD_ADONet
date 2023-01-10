using App.Domain;

namespace App.Domain.Contracts
{
    public interface IUserService
    {
        Task<User> CreateAsync(User user, CancellationToken cancellationToken);
        Task<User> DeleteAsync(User user, CancellationToken cancellationToken);
        Task<IList<User>> GetUsersAsync(CancellationToken cancellationToken);
        Task<User> UpdateAsync(User user, CancellationToken cancellationToken);
    }
}