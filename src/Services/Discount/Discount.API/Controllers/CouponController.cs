using Discount.API.Entites;
using Discount.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace Discount.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly ILogger<CouponController> _logger;
        private readonly ICouponRepository _repository;

        public CouponController(ILogger<CouponController> logger, ICouponRepository couponRepository)
        {
            _logger = logger;
            _repository = couponRepository;
        }

        [HttpGet]
        [Route("[action]/{productName}", Name = "GetDiscountByProductName")]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Coupon>> GetDiscountByProductName(string productName)
        {
            return await _repository.GetDiscount(productName);
        }


        [HttpPost]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> CreateCoupon([FromBody] Coupon coupon)
        {
            var result = await _repository.Create(coupon);

            return Ok(result);
        }


        [HttpPut]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> UpdateCoupon([FromBody] Coupon coupon)
        {
            var result = await _repository.Update(coupon);

            return Ok(result);
        }

        [HttpDelete("[action]/{productName}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> DeleteCoupon(string productName)
        {
            var result = await _repository.Delete(productName);

            return Ok(result);
        }


    }
}
