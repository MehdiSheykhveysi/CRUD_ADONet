using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Contracts
{
    public interface IProcedureFacade
    {
        Task<IList<T>> ExecuteQueryAync<T>(string query, CancellationToken cancellationToken);
        Task<T> ExecuteScalarQueryAync<T>(string query, Dictionary<string, string> procedureParameters, CancellationToken cancellationToken);
        Task<int> ExecuteNonQueryAsync(string procedureName, CancellationToken cancellationToken);
        Task<int> ExecuteNonQueryAsync(string procedureName, Dictionary<string, string> procedureParameters, CancellationToken cancellationToken);
        Task<T> ExecuteScalarAsync<T>(string procedureName, CancellationToken cancellationToken);
        Task<T> ExecuteScalarAsync<T>(string procedureName, Dictionary<string, string> procedureParameters, CancellationToken cancellationToken) where T : struct;
        Task<IList<T>> ExecuteWithResultAsync<T>(string prodedureName, CancellationToken cancellationToken) where T : class;
        Task<IList<T>> ExecuteWithResultAsync<T>(string prodedureName, Dictionary<string, string> procedureParameters, CancellationToken cancellationToken) where T : class;
    }
}
