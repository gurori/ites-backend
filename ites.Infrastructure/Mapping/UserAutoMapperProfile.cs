using AutoMapper;
using ites.Application.Contracts.Users;
using ites.Core.Entities;
using ites.Core.Models;

namespace ites.Infrastructure.Mapping
{
    public class UserAutoMapperProfile : Profile
    {
        public UserAutoMapperProfile()
        {
            CreateMap<User, User>();

            CreateMap<Core.Models.User, UserProfileResponse>();
        }
    }
}
