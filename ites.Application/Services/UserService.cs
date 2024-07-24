using AutoMapper;
using ites.Application.Contracts.Users;
using ites.Application.Interfaces.Auth;
using ites.Application.Interfaces.Repositories;
using ites.Application.Interfaces.Services;
using ites.Core.Enums;
using ites.Core.Models;
using ites.Core.Problems;
using Microsoft.IdentityModel.Tokens;

namespace ites.Application.Services
{
    public class UserService(IPasswordHasher passwordHasher,
                             IUserRepository userRepository,
                             IJwtProvider jwtProvider,
                             IMapper mapper) 
        : IUserService
    {
        private readonly IPasswordHasher _passwordHasher = passwordHasher;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IJwtProvider _jwtProvider = jwtProvider;
        private readonly IMapper _mapper = mapper;

        public async Task RegisterAsync(string name, string email, string password, string role)
        {
            var hashedPassword = _passwordHasher.Generate(password);

            User user = new(Guid.NewGuid(), name, email, hashedPassword, role);
            bool isUserExist = !await _userRepository.CreateAsync(user);

            if (isUserExist)
                throw UserProblem.UserAlreadyExist;
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var userEntity = await _userRepository.GetByEmailAsync(email)
                ?? throw UserProblem.NotExistEmail;

            if (!_passwordHasher.IsVerify(password, userEntity.PasswordHash))
                throw UserProblem.WrongPassword;

            var user = _mapper.Map<User>(userEntity);
            var token = await _jwtProvider.GenerateTokenAsync(user);

            return token;
        }

        public async Task<UserProfileResponse> GetFromTokenAsync(string token)
        {
            Guid id = await GetIdFromTokenAsync(token);
            User user = await _userRepository
                .GetByIdAsync(id);

            return _mapper.Map<UserProfileResponse>(user);
        }

        public async Task<UserProfileResponse> GetAsync(Guid id)
        {
            User user = await _userRepository.GetByIdAsync(id);
            return _mapper.Map<UserProfileResponse>(user);
        }

        public async Task UpdateAsync(
            Guid id, string lastName, string firstName, string middleName, string description, string jobTitle)
        {
            await _userRepository
                .UpdateAsync(id, lastName, firstName, middleName, description, jobTitle);
        }

        public async Task<Guid> GetIdFromTokenAsync(string token)
        {
            TokenValidationResult validationResult = await _jwtProvider
                .ValidateTokenAsync(token);

            if (!validationResult.IsValid)
                throw UserProblem.TokenProblem;

            string id = validationResult.Claims[CustomClaims.UserId].ToString()
                ?? throw UserProblem.TokenProblem;

            return Guid.Parse(id);
        }
    }
}
