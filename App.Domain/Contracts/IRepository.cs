using App.Domain.Shared;

namespace App.Domain.Contracts
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        Task<IList<TEntity>> GetEntitiesAsync(CancellationToken cancellationToken);
    }
}
