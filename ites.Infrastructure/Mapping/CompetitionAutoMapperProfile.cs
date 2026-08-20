using AutoMapper;
using ites.Application.Contracts.Competitions;
using ites.Core.Entities;
using ites.Core.Models;

namespace ites.Infrastructure.Mapping
{
    public class CompetitionAutoMapperProfile : Profile
    {
        public CompetitionAutoMapperProfile()
        {
            CreateMap<Competition, Competition>();

            CreateMap<Core.Models.Competition, CompetitionResponse>();
        }
    }
}
