using AutoMapper;
using ites.Application.Contracts.Users;
using ites.Core.Entities;

namespace ites.Infrastructure.Mapping
{
    public class UserAutoMapperProfile : Profile
    {
        public UserAutoMapperProfile()
        {

            CreateMap<User, UserProfileResponse>();
        }
    }
}
