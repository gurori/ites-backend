using AutoMapper;
using ites.DataAccess.Entites;

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
