using _04.App.infrastructure.SqlRepositories;
using App.Domain;
using App.Domain.Contracts;
using App.Domain.Shared;
using App.infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.infrastructure.Shared
{
    public class SqlUnitOfWork : IUnitOfWork
    {
        private bool disposedValue;
        private readonly SqlConnection Context;
        private readonly SqlTransaction Transaction;

        public SqlUnitOfWork(SqlConnection sqlConnection)
        {
            Context = sqlConnection;
            Context.Open();

            Transaction = Context.BeginTransaction();
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            return Transaction.CommitAsync(cancellationToken);
        }

        protected SqlCommand CreateCommand(string query) => new(query, Context, Transaction);

        public async Task<IList<T>> ExecuteQueryAync<T>(string query, CancellationToken cancellationToken)
        {
            var command = CreateCommand(query);
            return await command
                .LoadQuery(query)
                          .ExecuteCommandAsync<T>(cancellationToken);
        }
        public Task<T> ExecuteScalarQueryAync<T>(string query, Dictionary<string, string> procedureParameters, CancellationToken cancellationToken)
        {
            var command = CreateCommand(query);

            foreach (var item in procedureParameters)
            {
                command.WithSqlParam(item.Key, item.Value);
            }
            return command
                          .ExecuteCommandScalarAsync<T>(cancellationToken);
        }
        public async Task<IList<T>> ExecuteWithResultAsync<T>(string prodedureName, Dictionary<string, string> procedureParameters, CancellationToken cancellationToken) where T : class
        {
            var loadedProcedure = CreateCommand(prodedureName).LoadStoredProc(prodedureName);
            foreach (var item in procedureParameters)
            {
                loadedProcedure.WithSqlParam(item.Key, item.Value);
            }
            var result = await loadedProcedure.ExecuteCommandAsync<T>(cancellationToken);
            return result;
        }
        public async Task<IList<T>> ExecuteWithResultAsync<T>(string prodedureName, CancellationToken cancellationToken) where T : class
        {
            var result = await CreateCommand(prodedureName).LoadStoredProc(prodedureName).ExecuteCommandAsync<T>(cancellationToken);
            return result;
        }

        public async Task<T> ExecuteScalarAsync<T>(string procedureName, CancellationToken cancellationToken)
        {
            return (T)await CreateCommand(procedureName).LoadStoredProc(procedureName).ExecuteCommandAsync<T>(cancellationToken);
        }

        public async Task<T> ExecuteScalarAsync<T>(string procedureName, Dictionary<string, string> procedureParameters, CancellationToken cancellationToken) where T : struct
        {
            var procedure = CreateCommand(procedureName).LoadStoredProc(procedureName);
            foreach (var item in procedureParameters)
            {
                procedure.WithSqlParam(item.Key, item.Value);
            }
            return (T)await procedure.ExecuteCommandScalarAsync<T>(cancellationToken);
        }

        public async Task<int> ExecuteNonQueryAsync(string procedureName, CancellationToken cancellationToken)
        {
            return await CreateCommand(procedureName).LoadStoredProc(procedureName).ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task<int> ExecuteNonQueryAsync(string procedureName, Dictionary<string, string> procedureParameters, CancellationToken cancellationToken)
        {
            var procedure = CreateCommand(procedureName).LoadStoredProc(procedureName);
            foreach (var item in procedureParameters)
            {
                procedure.WithSqlParam(item.Key, item.Value);
            }
            return await procedure.ExecuteNonQueryAsync(cancellationToken);
        }


        protected async virtual Task Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    await Context.CloseAsync();
                    await Transaction.DisposeAsync();
                    await Context.DisposeAsync();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~SqlUnitOfWork()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public async ValueTask DisposeAsync()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            await Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

    }
}
