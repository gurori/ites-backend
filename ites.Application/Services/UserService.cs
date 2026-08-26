using ites.Application.Contracts.Users;
using ites.Application.Interfaces.Auth;
using ites.Application.Interfaces.Services;
using ites.Core.Entities;
using ites.Core.Exceptions;
using ites.Core.Interfaces.Repositories;

namespace ites.Application.Services;

public sealed class UserService(
    IPasswordHasher passwordHasher,
    IUserRepository userRepository,
    IJwtProvider jwtProvider
) : IUserService
{
    public async Task RegisterAsync(RegisterUserRequest request, CancellationToken ct = default)
    {
        bool userExist = await userRepository.AnyAsync(u => u.Email == request.Email, ct);

        if (userExist)
            throw new ConflictException("Пользователь с данной почтой уже зарегистрирован");

        string hashedPassword = passwordHasher.Generate(request.Password);

        User user = new()
        {
            Id = Guid.CreateVersion7(),
            Email = request.Email,
            FirstName = request.FirstName,
            PasswordHash = hashedPassword,
            Role = request.Role,
        };

        await userRepository.CreateAsync(user, ct);
        await userRepository.SaveChangesAsync(ct);
    }

    public async Task<LoginUserResponse> LoginAsync(
        LoginUserRequest request,
        CancellationToken ct = default
    )
    {
        var userEntity = await userRepository.GetByEmailAsync(
            request.Email,
            u => new
            {
                u.Id,
                u.PasswordHash,
                u.Role,
            },
            ct
        );

        if (userEntity == null || !passwordHasher.Verify(request.Password, userEntity.PasswordHash))
        {
            throw new UnauthorizedException("Неверный email или пароль");
        }

        var token = await jwtProvider.GenerateTokenAsync(userEntity.Id, userEntity.Role, ct);

        return new LoginUserResponse(token, userEntity.Role);
    }

    public async Task UpdateAsync(
        Guid id,
        UpdateUserRequest request,
        CancellationToken ct = default
    )
    {
        var user =
            await userRepository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException("Пользователь не найден");

        user.LastName = request.LastName;
        user.FirstName = request.FirstName;
        user.MiddleName = request.MiddleName;
        user.Description = request.Description;
        user.JobTitle = request.JobTitle ?? "";

        await userRepository.UpdateAsync(user, ct);
        await userRepository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid userId, CancellationToken ct = default)
    {
        await userRepository.DeleteAsync(userId, ct);
    }
}
