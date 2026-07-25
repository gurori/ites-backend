using AutoMapper;
using ites.Application.Contracts.Users;
using ites.Core.Models;
using ites.Core.Entities;

namespace ites.Infastructure.Mapping
{
    public class UserAutoMapperProfile : Profile
    {
        public UserAutoMapperProfile()
        {
            CreateMap<UserEntity, User>();

            CreateMap<User, UserProfileResponse>();
        }
    }
}
