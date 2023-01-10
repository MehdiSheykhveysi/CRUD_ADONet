using App.Domain;
using App.Domain.Contracts;

namespace App.ApplicationServices.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public Task<IList<User>> GetUsersAsync(CancellationToken cancellationToken)
        {
            return userRepository.GetEntitiesAsync(cancellationToken);
        }

        public Task<User> CreateAsync(User user, CancellationToken cancellationToken)
        {
            return userRepository.CreateAsync(user, cancellationToken);
        }

        public Task<User> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            return userRepository.UpdateAsync(user, cancellationToken);
        }

        public Task<User> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            return userRepository.DeleteAsync(user, cancellationToken);
        }
    }
}
