using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private IProductRepository _repository;
        private ILogger<CatalogController> _logger;

        public CatalogController(ILogger<CatalogController> logger, IProductRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }


        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            await _repository.Create(product);

            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }


        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            return Ok(await _repository.Update(product));
        }


        [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProductById(string id)
        {
            return Ok(await _repository.Delete(id));
        }


        [HttpGet]
        [ProducesResponseType(typeof(IList<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IList<Product>>> GetAll()
        {
            return Ok(await _repository.GetAll());
        }


        [HttpGet]
        [Route("[action]/{name}", Name = "GetProductsByName")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IList<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IList<Product>>> GetProductsByName(string name)
        {
            var product = await _repository.GetByName(name);

            if (product == null)
            {
                _logger.LogError($"Product with name: {name}, not found");
                return NotFound();
            }

            return Ok(product);
        }


        [HttpGet]
        [Route("[action]/{category}", Name = "GetProductsByCategory")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IList<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IList<Product>>> GetProductsByCategory(string category)
        {
            var product = await _repository.GetByCategory(category);

            if (product == null)
            {
                _logger.LogError($"Product with category: {category}, not found");
                return NotFound();
            }

            return Ok(product);
        }


        [HttpGet]
        [Route("[action]/{id}", Name = "GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            var product = await _repository.GetById(id);

            if (product == null)
            {
                _logger.LogError($"Product with id: {id}, not found");
                return NotFound();
            }

            return Ok(product);
        }
    }
}
