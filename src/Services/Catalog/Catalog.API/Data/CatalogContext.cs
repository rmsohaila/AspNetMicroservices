using Catalog.API.Data.Interfaces;
using Catalog.API.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(IConfiguration config)
        {
            var client = new MongoClient(config.GetValue<string>("ConnectionStrings:MongoConnectionString"));
            var database = client.GetDatabase(config.GetValue<string>("ConnectionStrings:DatabaseName"));
            Products = database.GetCollection<Product>(config.GetValue<string>("ConnectionStrings:ProductCollectionName"));
            CatalogSeeder.SeedData(Products);
        }

        public IMongoCollection<Product> Products { get; }
    }
}
