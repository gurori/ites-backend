using AutoMapper;
using ites.Application.Contracts.Users;
using ites.Core.Enums;
using ites.Core.Models;
using ites.DataAccess.Entites;

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
