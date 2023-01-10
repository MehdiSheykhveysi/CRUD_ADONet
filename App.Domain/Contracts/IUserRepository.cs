using App.Domain;

namespace App.Domain.Contracts
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> CreateAsync(User user, CancellationToken cancellationToken);
        Task<User> DeleteAsync(User user, CancellationToken cancellationToken);
        Task<User> UpdateAsync(User user, CancellationToken cancellationToken);
    }
}