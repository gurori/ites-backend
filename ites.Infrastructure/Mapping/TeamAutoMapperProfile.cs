using AutoMapper;
using ites.Application.Contracts.Teams;
using ites.Core.Entities;
using ites.Core.Models;

namespace ites.Infrastructure.Mapping
{
    public class TeamAutoMapperProfile : Profile
    {
        public TeamAutoMapperProfile()
        {
            CreateMap<Team, Team>();

            CreateMap<Core.Models.Team, TeamResponse>();
        }
    }
}
