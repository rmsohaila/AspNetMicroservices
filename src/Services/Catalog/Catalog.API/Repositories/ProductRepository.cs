using Catalog.API.Data.Interfaces;
using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;

        public ProductRepository(ICatalogContext context)
        {
            this._context = context;
        }

        public async Task Create(Product product)
        {
            await _context.Products.InsertOneAsync(product);
        }

        public async Task<bool> Delete(string id)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
            var result = await _context
                .Products
                .DeleteOneAsync(filter);

            return result.IsAcknowledged
                && result.DeletedCount > 0;
        }

        public async Task<IList<Product>> GetAll()
        {
            return await _context
                .Products
                .Find(p => true)
                .ToListAsync();
        }

        public async Task<IList<Product>> GetByCategory(string category)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Category, category);

            return await _context
                .Products
                .Find(filter)
                .ToListAsync();
        }

        public async Task<Product> GetById(string id)
        {
            return await _context
                .Products
                .Find(p => p.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IList<Product>> GetByName(string name)
        {
            var queryExpr = new BsonRegularExpression(new Regex($"{name}", RegexOptions.IgnoreCase));
            var filter = Builders<Product>.Filter.Regex("Name", queryExpr);

            return await _context
                .Products
                .Find(filter)
                .ToListAsync();
        }

        public async Task<bool> Update(Product product)
        {
            var result = await _context
                .Products
                .ReplaceOneAsync(p => p.Id == product.Id, product);

            return result.IsAcknowledged
                && result.ModifiedCount > 0;
        }
    }
}
