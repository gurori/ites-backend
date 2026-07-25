using AutoMapper;
using ites.Application.Contracts.Teams;
using ites.Core.Models;
using ites.Core.Entities;

namespace ites.Infastructure.Mapping
{
    public class TeamAutoMapperProfile : Profile
    {
        public TeamAutoMapperProfile()
        {
            CreateMap<TeamEntity, Team>();

            CreateMap<Team, TeamResponse>();
        }
    }
}
