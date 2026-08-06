using AutoMapper;
using ites.Application.Contracts.Orders;
using ites.Core.Entities;
using ites.Core.Models;

namespace ites.Infrastructure.Mapping
{
    public class OrderAutoMapperProfile : Profile
    {
        public OrderAutoMapperProfile()
        {
            CreateMap<OrderEntity, Order>();

            CreateMap<Order, OrderResponse>();
        }
    }
}
