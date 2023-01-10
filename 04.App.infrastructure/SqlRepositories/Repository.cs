using App.Domain.Contracts;
using App.Domain.Shared;
using App.infrastructure.Extensions;
using PluralizeService.Core;
using System.Data.SqlClient;

namespace _04.App.infrastructure.SqlRepositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : IEntity
    {
        private readonly IUnitOfWork unitOfWork;
        public Repository(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IList<TEntity>> GetEntitiesAsync(CancellationToken cancellationToken)
        {
            var query = $"Select * from {PluralizationProvider.Pluralize(typeof(TEntity).Name)}";
            var entities = await unitOfWork.ExecuteQueryAync<TEntity>(query, cancellationToken);
            return entities;
        }
    }
}
