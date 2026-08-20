using AutoMapper;
using ites.Application.Contracts.Teams;
using ites.Core.Entities;

namespace ites.Infrastructure.Mapping
{
    public class TeamAutoMapperProfile : Profile
    {
        public TeamAutoMapperProfile()
        {

            CreateMap<Team, TeamResponse>();
        }
    }
}
