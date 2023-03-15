using Dapper;
using Discount.GRPC.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Threading.Tasks;

namespace Discount.GRPC.Data
{
    public class PGContext : IPGContext
    {
        private readonly IConfiguration _config;

        public PGContext(IConfiguration config)
        {
            _config = config;
        }

        public async Task<int> ExecuteQueryAsync(string query, object parameters)
        {
            using var conn = new NpgsqlConnection(_config.GetValue<string>("ConnectionStrings:PGConnectionString"));
            
            var affected = await conn.ExecuteAsync(query, parameters);

            return affected;
        }

        public int ExecuteQuery(string query, object parameters)
        {
            using var conn = new NpgsqlConnection(_config.GetValue<string>("ConnectionStrings:PGConnectionString"));

            var affected = conn.Execute(query, parameters);

            return affected;
        }

        async Task<dynamic> IPGContext.SelectQueryAsync(string query, object parameters)
        {
            using var conn = new NpgsqlConnection(_config.GetValue<string>("ConnectionStrings:PGConnectionString"));

            return await conn.QueryFirstOrDefault(query, parameters);
        }

        dynamic IPGContext.SelectQuery(string query, object parameters)
        {
            throw new System.NotImplementedException();
        }
    }
}
