using AutoMapper;
using ites.Core.Entities;

namespace ites.Infrastructure.Mapping
{
    public class ApplicationAutoMapperProfile : Profile
    {
        public ApplicationAutoMapperProfile()
        {
            CreateMap<Application, Core.Models.Application>();
        }
    }
}
