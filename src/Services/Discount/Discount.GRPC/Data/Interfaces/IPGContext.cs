using Npgsql;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discount.GRPC.Data.Interfaces
{
    public interface IPGContext
    {
        int ExecuteQuery(string query, object parameters);
        Task<int> ExecuteQueryAsync(string query, object parameters);
        Task<dynamic> SelectQueryAsync(string query, object parameters);
        dynamic SelectQuery(string query, object parameters);
    }
}
