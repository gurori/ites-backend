using AutoMapper;
using ites.Application.Contracts.Orders;
using ites.Core.Entities;

namespace ites.Infrastructure.Mapping
{
    public class OrderAutoMapperProfile : Profile
    {
        public OrderAutoMapperProfile()
        {

            CreateMap<Order, OrderResponse>();
        }
    }
}
