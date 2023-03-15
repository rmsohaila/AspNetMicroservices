using AutoMapper;
using Dicount.GRPC.Protos;
using Discount.API.Repositories.Interfaces;
using Discount.GRPC.Entites;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Dicount.GRPC.Services
{
    public class DiscountCouponService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly ICouponRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DiscountCouponService> _logger;

        public DiscountCouponService(
            ICouponRepository repository,
            IMapper mapper,
            ILogger<DiscountCouponService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _repository.GetDiscount(request.ProductName);
            if (coupon == null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with Product Name: {request.ProductName} is not found"));

            _logger.LogInformation("Discount for {productName} is retrieved with Amount: ", coupon.ProductName, coupon.Amount);

            var couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);

            var success = await _repository.Create(coupon);
            if (!success)
                throw new RpcException(new Status(StatusCode.Unknown, $"Error creating coupon for product: {request.Coupon.ProductName}"));

            var couponModel = _mapper.Map<CouponModel>(coupon);

            return couponModel;
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);

            var success = await _repository.Update(coupon);
            if (!success)
                throw new RpcException(new Status(StatusCode.Internal, $"Error updating coupon for product: {request.Coupon.ProductName}"));

            var couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var success = await _repository.Delete(request.ProductName);
            var response = new DeleteDiscountResponse
            {
                Success = success
            };

            return response;
        }
    }
}
