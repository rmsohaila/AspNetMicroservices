using Catalog.API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.API.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IList<Product>> GetAll();
        Task<Product> GetById(string id);
        Task<IList<Product>> GetByName(string name);
        Task<IList<Product>> GetByCategory(string category);

        Task Create(Product product);
        Task<bool> Update(Product product);
        Task<bool> Delete(string id);
    }
}
