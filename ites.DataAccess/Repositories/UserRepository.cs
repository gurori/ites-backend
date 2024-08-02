using AutoMapper;
using ites.Application.Interfaces.Repositories;
using ites.Core.Models;
using ites.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;

namespace ites.DataAccess.Repositories
{
    public class UserRepository(ItesDbContext context, IMapper mapper) 
        : IUserRepository
    {
        private readonly ItesDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<bool> CreateAsync(User user)
        {
            var userRequest = await GetUserEntityByEmailAsync(user.Email);

            if(userRequest is not null) return false;

            var userEntity = new UserEntity()
            {
                Id = Guid.NewGuid(),
                FirstName = user.FirstName,
                PasswordHash = user.PasswordHash,
                Email = user.Email,
                Role = user.Role
            };

            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var userEntity = await GetUserEntityByEmailAsync(email);

            return userEntity is null 
                ? null : _mapper.Map<User>(userEntity);
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            var userEntity = await GetUserEntityByIdAsync(id);

            return _mapper.Map<User>(userEntity);
        }

        public async Task<string?> GetRoleByIdAsync(Guid id)
        {
            var userEntity = await GetUserEntityByIdAsync(id);

            return userEntity?.Role;
        }

        public async Task UpdateAsync(
            Guid id, string lastName, string firstName, string middleName, string description, string jobTitle)
        {
            await _context.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(u => u.LastName, u => lastName)
                    .SetProperty(u => u.FirstName, u => firstName)
                    .SetProperty(u => u.MiddleName, u => middleName)
                    .SetProperty(u => u.Description, u => description)
                    .SetProperty(u => u.JobTitle, u => jobTitle));

            await _context.SaveChangesAsync();
        }

        private async Task<UserEntity?> GetUserEntityByEmailAsync(string email) =>
            await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);

        private async Task<UserEntity?> GetUserEntityByIdAsync(Guid id) =>
            await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
    }
}
