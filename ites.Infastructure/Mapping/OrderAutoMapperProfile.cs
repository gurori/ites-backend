using AutoMapper;
using ites.Application.Contracts.Orders;
using ites.Core.Models;
using ites.DataAccess.Entites;

namespace ites.Infastructure.Mapping
{
    public class OrderAutoMapperProfile
        : Profile
    {
        public OrderAutoMapperProfile()
        {
            CreateMap<OrderEntity, Order>();

            CreateMap<Order, OrderResponse>();
        }
    }
}
