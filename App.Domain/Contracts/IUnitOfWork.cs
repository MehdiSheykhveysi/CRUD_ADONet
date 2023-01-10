using App.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Contracts
{
    public interface IUnitOfWork : IAsyncDisposable, IProcedureFacade
    {
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
