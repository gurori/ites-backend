using AutoMapper;
using ites.Application.Interfaces.Repositories;
using ites.Core.Models;
using ites.DataAccess;
using ites.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;

namespace ites.Persistence.Repositories
{
    public class UserRepository(ItesDbContext context, IMapper mapper) : IUserRepository
    {
        private readonly ItesDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<bool> Create(User user)
        {
            var userRequest = await GetUserEntityByEmail(user.Email);

            if (userRequest is not null) return false;

            var userEntity = new UserEntity()
            {
                Id = Guid.NewGuid(),
                Name = user.Name,
                PasswordHash = user.PasswordHash,
                Email = user.Email,
            };

            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User?> GetByEmail(string email)
        {
            var userEntity = await GetUserEntityByEmail(email);

            return userEntity is null ? null : _mapper.Map<User>(userEntity);
        }

        public async Task<User> GetById(Guid id)
        {
            var userEntity = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

            return _mapper.Map<User>(userEntity);
        }
        private async Task<UserEntity?> GetUserEntityByEmail(string email) =>
            await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);
    }
}
