using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace App.infrastructure.Extensions
{
    public static class SqlDbCommandExtension
    {
        public static DbCommand WithSqlParam(this DbCommand cmd, string paramName, object paramValue)
        {
            if (string.IsNullOrEmpty(cmd.CommandText))
                throw new InvalidOperationException("Call LoadStoredProc before using this method");

            var param = cmd.CreateParameter();
            param.ParameterName = paramName;
            param.Value = paramValue;
            cmd.Parameters.Add(param);
            return cmd;
        }

        public static DbCommand LoadStoredProc(this DbCommand cmd, string storedProcName)
        {
            cmd.CommandText = storedProcName;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            return cmd;
        }

        public static DbCommand LoadQuery(this DbCommand cmd, string commandText)
        {
            cmd.CommandText = commandText;
            cmd.CommandType = System.Data.CommandType.Text;
            return cmd;
        }

        public static IList<T> MapToList<T>(this DbDataReader dr)
        {
            var objList = new List<T>();
            var props = typeof(T).GetRuntimeProperties();

            var colMapping = dr.GetColumnSchema()
                .Where(x => props.Any(y => y.Name.ToLower() == x.ColumnName.ToLower()))
                .ToDictionary(key => key.ColumnName.ToLower());

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    T obj = Activator.CreateInstance<T>();
                    foreach (var prop in props)
                    {
                        var val = dr.GetValue(colMapping[prop.Name.ToLower()].ColumnOrdinal.Value);
                        prop.SetValue(obj, val == DBNull.Value ? null : val);
                    }
                    objList.Add(obj);
                }
            }
            return objList;
        }

        public static async Task<IList<T>> ExecuteCommandAsync<T>(this DbCommand command, CancellationToken cancellationToken)
        {
            using (command)
            {
                if (command.Connection.State == System.Data.ConnectionState.Closed)
                    await command.Connection.OpenAsync(cancellationToken);
                try
                {
                    using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                    {
                        return reader.MapToList<T>();
                    }
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
        }
        public static async Task<T> ExecuteCommandScalarAsync<T>(this DbCommand command, CancellationToken cancellationToken)
        {
            using (command)
            {
                if (command.Connection.State == System.Data.ConnectionState.Closed)
                    await command.Connection.OpenAsync(cancellationToken);
                try
                {
                    return (T)await command.ExecuteScalarAsync(cancellationToken);
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
