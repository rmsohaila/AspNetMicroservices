using AutoMapper;
using Discount.GRPC.Entites;
using Dicount.GRPC.Protos;

namespace Dicount.GRPC.Mapper
{
    public class DiscountProfile : Profile
    {
        public DiscountProfile()
        {
            CreateMap<Coupon, CouponModel>().ReverseMap();
        }
    }
}
