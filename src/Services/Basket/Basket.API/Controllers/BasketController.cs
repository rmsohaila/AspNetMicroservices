using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        private readonly ILogger<ControllerBase> _logger;
        private readonly DiscountGrpcService _discountGrpcService;

        public BasketController(
            IBasketRepository repository, 
            ILogger<ControllerBase> logger, 
            DiscountGrpcService discountGrpcService)
        {
            _repository = repository;
            _logger = logger;
            _discountGrpcService = discountGrpcService;
        }

        [HttpGet("{username}", Name = "GetBasketByUsername")]
        [ProducesResponseType(typeof(IList<ShoppingCart>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasketByUsername(string username)
        { 
            var basket = await _repository.GetBasket(username);

            return Ok(basket ?? new ShoppingCart(username));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart cart)
        {
            foreach (var product in cart.Products)
            {
                var coupon = await _discountGrpcService.GetDiscount(product.Name);
                product.Price -= coupon.Amount;
            }

            return Ok(await _repository.UpdateBasket(cart));
        }

        [HttpDelete]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string username)
        {
            await _repository.DeleteBasket(username);

            return Ok();
        }
    }
}
