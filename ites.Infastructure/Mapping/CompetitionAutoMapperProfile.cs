using AutoMapper;
using ites.Core.Models;
using ites.Application.Contracts.Competitions;
using ites.Core.Entities;

namespace ites.Infastructure.Mapping
{
    public class CompetitionAutoMapperProfile : Profile
    {
        public CompetitionAutoMapperProfile()
        {
            CreateMap<CompetitionEntity, Competition>();

            CreateMap<Competition, CompetitionResponse>();
        }
    }
}
