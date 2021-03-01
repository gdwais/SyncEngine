using SyncEngine.Core.Configuration;
using Microsoft.Extensions.Options;
using Dapper;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SyncEngine.Core
{
    public class Repository
    {
        private readonly ConnectionStrings connStrings;

        public Repository(IOptions<ConnectionStrings> connectionStrings)
        {
            connStrings = connectionStrings.Value;
        }

        protected IDbConnection GetOpenConnection()
        {
            var connection = new SqlConnection(connStrings.SyncEngine);
            connection.Open();
            return connection;
        }

        protected async Task<IEnumerable<TReturns>> QueryAsync<TFirst, TSecond, TReturns>(string sql, Func<TFirst, TSecond, TReturns> map, object parameters, string splitOn = "Id")
        {
            using var c = GetOpenConnection();
            return await c.QueryAsync(sql, map, param: parameters, splitOn: splitOn);
        }

        protected async Task<dynamic> QuerySingleAsync(string sql, object parameters)
        {
            using var c = GetOpenConnection();
            return await c.QuerySingleAsync(sql, parameters);
        }

        protected async Task<T> QuerySingleAsync<T>(string sql, object parameters)
        {
            using var c = GetOpenConnection();
            return await c.QuerySingleAsync<T>(sql, parameters);
        }

        protected async Task ExecuteAsync(string sql, object parameters)
        {
            using var c = GetOpenConnection();
            await c.ExecuteAsync(sql, parameters);
        }

    }
}