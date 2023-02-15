using Dapper;
using Discount.API.Entites;
using Discount.API.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Threading.Tasks;

namespace Discount.API.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly ILogger<CouponRepository> _logger;
        private readonly string ConnectionString;

        public CouponRepository(IConfiguration config, ILogger<CouponRepository> logger)
        {
            _logger = logger;
            ConnectionString = config.GetValue<string>("ConnectionStrings:PGConnectionString");
        }

        public async Task<bool> Create(Coupon coupon)
        {
            try
            {
                using var conn = new NpgsqlConnection(ConnectionString);
                var affected = await conn.ExecuteAsync("INSERT INTO Coupons (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
                    new
                    {
                        ProductName = coupon.ProductName,
                        Description = coupon.Description,
                        Amount = coupon.Amount
                    });

                if (affected == 0) return false;

                return true;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(ex, $"Error creating coupon record");
                return false;
            }
        }

        public async Task<bool> Delete(string productName)
        {
            using var conn = new NpgsqlConnection(ConnectionString);

            var affected = await conn.ExecuteAsync("DELETE FROM Coupons WHERE ProductName = @ProductName", new { ProductName = productName });

            if (affected == 0) return false;

            return true;
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            using var conn = new NpgsqlConnection(ConnectionString);

            var coupon = await conn.QueryFirstOrDefaultAsync<Coupon>
                ("SELECT * FROM Coupons WHERE ProductName = @ProductName", new { ProductName = productName });

            if (coupon == null) return new Coupon { ProductName = "No Discount", Description = "No Discount Description", Amount = 0 };

            return coupon;
        }

        public async Task<bool> Update(Coupon coupon)
        {
            using var conn = new NpgsqlConnection(ConnectionString);

            var affected = await conn.ExecuteAsync(@"UPDATE Coupons 
                                                     SET 
                                                        ProductName = @ProductName,
                                                        Description = @Description,
                                                        Amount = @Amount
                                                     WHERE 
                                                        ProductName = @ProductName",
                                                        new
                                                        {
                                                            ProductName = coupon.ProductName,
                                                            Description = coupon.Description,
                                                            Amount = coupon.Amount,
                                                        });

            if (affected == 0) return false;

            return true;
        }
    }
}
