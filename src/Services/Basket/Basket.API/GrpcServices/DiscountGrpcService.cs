using Dicount.GRPC.Protos;
using System.Threading.Tasks;
using static Dicount.GRPC.Protos.DiscountProtoService;

namespace Basket.API.GrpcServices
{
    public class DiscountGrpcService
    {
        private readonly DiscountProtoServiceClient _service;

        public DiscountGrpcService(DiscountProtoServiceClient service)
        {
            _service = service;
        }

        public async Task<CouponModel> GetDiscount(string productName)
        {
            var discountRequest = new GetDiscountRequest { ProductName = productName };

            return await _service.GetDiscountAsync(discountRequest);
        }
    }
}
