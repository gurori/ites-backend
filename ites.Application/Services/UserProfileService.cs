using ites.Application.Contracts.Users;
using ites.Application.Interfaces.Services;
using ites.Application.Mapping;
using ites.Core.Exceptions;
using ites.Core.Interfaces.Repositories;

namespace ites.Application.Services;

public sealed class UserProfileService(IUserRepository userRepository) : IUserProfileService
{
    public async Task<MemberResponse> GetMemberAsync(Guid id, CancellationToken ct = default)
    {
        var member =
            await userRepository.GetByIdAsync(id, UserMapping.ToMemberResponse, ct)
            ?? throw new NotFoundException("Пользователь не найден.");

        return member;
    }

    public async Task<OrganizerResponse> GetOrganizerAsync(Guid id, CancellationToken ct = default)
    {
        var organizer =
            await userRepository.GetByIdAsync(id, UserMapping.ToOrganizerResponse, ct)
            ?? throw new NotFoundException("Пользователь не найден.");

        return organizer;
    }

    public async Task<ClientResponse> GetClientAsync(Guid id, CancellationToken ct = default)
    {
        var client =
            await userRepository.GetByIdAsync(id, UserMapping.ToClientResponse, ct)
            ?? throw new NotFoundException("Пользователь не найден.");

        return client;
    }
}
