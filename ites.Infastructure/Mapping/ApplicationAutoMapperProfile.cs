using AutoMapper;
using ites.Core.Entities;

namespace ites.Infastructure.Mapping
{
    public class ApplicationAutoMapperProfile
        : Profile
    {
        public ApplicationAutoMapperProfile()
        {
            CreateMap<ApplicationEntity, Core.Models.Application>();
        }
    }
}
