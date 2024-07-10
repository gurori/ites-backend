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
                //.ConstructUsing(ue => new User(ue.Id, ue.Name, ue.Email, ue.PasswordHash, ue.Roles.FirstOrDefault(r => r.Name != Role.User.ToString())!.Name));

            CreateMap<User, UserProfileResponse>();
        }
    }
}
