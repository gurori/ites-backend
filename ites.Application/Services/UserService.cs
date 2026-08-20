using AutoMapper;
using ites.Application.Contracts.Users;
using ites.Application.Interfaces.Auth;
using ites.Application.Interfaces.Services;
using ites.Core.Exeptions;
using ites.Core.Interfaces.Repositories;
using ites.Core.Models;
using Microsoft.Extensions.Options;

namespace ites.Application.Services
{
    public class UserService(
        IPasswordHasher passwordHasher,
        IUserRepository userRepository,
        IJwtProvider jwtProvider,
        IMapper mapper
    ) : IUserService
    {
        private readonly IPasswordHasher _passwordHasher = passwordHasher;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IJwtProvider _jwtProvider = jwtProvider;
        private readonly IMapper _mapper = mapper;

        public async Task RegisterAsync(string name, string email, string password, string role)
        {
            string hashedPassword = _passwordHasher.Generate(password);

            User user = new(Guid.NewGuid(), name, email, hashedPassword, role);
            bool isUserExist = !await _userRepository.CreateAsync(user);

            if (isUserExist)
                throw new ConflictException("Данный пользователь уже существует");
        }

        public async Task<LoginUserResponse> LoginAsync(string email, string password)
        {
            var userEntity =
                await _userRepository.GetByEmailAsync(email)
                ?? throw new NotFoundException("Пользователь с данной почтой не зарегистрирован");

            if (!_passwordHasher.IsVerify(password, userEntity.PasswordHash))
                throw new ConflictException("Неверный пароль");

            var user = _mapper.Map<User>(userEntity);
            var token = await _jwtProvider.GenerateTokenAsync(user);

            return new LoginUserResponse(token, user.Role);
        }

        public async Task<UserProfileResponse> GetAsync(Guid id)
        {
            User? user = await _userRepository.GetByIdAsync(id);
            return _mapper.Map<UserProfileResponse>(user);
        }

        public async Task UpdateAsync(
            Guid id,
            string lastName,
            string firstName,
            string middleName,
            string description,
            string? jobTitle
        )
        {
            await _userRepository.UpdateAsync(
                id,
                lastName,
                firstName,
                middleName,
                description,
                jobTitle ?? string.Empty
            );
        }

        public async Task<IList<UserProfileResponse>> GetManyAsync(ICollection<Guid> ids)
        {
            IList<User> users = await _userRepository.GetManyByIdAsync(ids);
            return _mapper.Map<UserProfileResponse[]>(users);
        }

        public async Task DeleteAsync(Guid userId)
        {
            await _userRepository.DeleteByIdAsync(userId);
        }
    }
}
