using AutoMapper;
using ites.Application.Contracts.Competitions;
using ites.Core.Entities;

namespace ites.Infrastructure.Mapping
{
    public class CompetitionAutoMapperProfile : Profile
    {
        public CompetitionAutoMapperProfile()
        {

            CreateMap<Competition, CompetitionResponse>();
        }
    }
}
