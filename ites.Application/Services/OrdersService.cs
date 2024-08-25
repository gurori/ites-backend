using AutoMapper;
using ites.Application.Interfaces.Auth;
using ites.Application.Interfaces.Repositories;
using ites.Application.Interfaces.Services;
using ites.Core.Enums;
using ites.Core.Models;
using ites.Core.Problems;
using Microsoft.IdentityModel.Tokens;

namespace ites.Application.Services
{
    public sealed class OrdersService(
        IOrdersRepository ordersRepository,
        IJwtProvider jwtProvider,
        IApplicationsRepository applicationsRepository)
            : IOrdersService
    {
        private readonly IOrdersRepository _ordersRepository = ordersRepository;
        private readonly IApplicationsRepository _applicationsRepository = applicationsRepository;
        private readonly IJwtProvider _jwtProvider = jwtProvider;

        public async Task AddApplicationAsync(string token, Guid forId)
        {
            Guid fromId = await GetUserIdFromTokenAsync(token);
            Core.Models.Application application = new(Guid.Empty, fromId, forId);
            await _applicationsRepository
                .CreateForOrderAsync(application);
        }

        public async Task CreateAsync(string token, string title, string description, decimal price, DateTime deadLine)
        {
            Guid clientId = await GetUserIdFromTokenAsync(token);
            Order order = new(Guid.Empty, title, description, price, deadLine);
            await _ordersRepository.CreateAsync(clientId, order);
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Order> GetAsync(Guid id)
        {
            Order order = await _ordersRepository
                .GetByIdAsync(id)
                    ?? throw OrderProblem.NotFound;
            return order;
        }

        public async Task<IList<Order>> GetAsync()
        {
            return await _ordersRepository
                .GetAllAsync();
        }

        public async Task<IList<Order>> GetAsync(IList<Guid> ids)
        {
            return await _ordersRepository
                .GetWithIdsAsync(ids);
        }

        public async Task HandleApplicationAsync(Guid id, bool isAccept)
        {
            await _applicationsRepository
                .HandleOrderAsync(id, isAccept);
        }

        public Task UpdateAsync(string token, Guid id, string title, string description, decimal price, DateTime deadLine)
        {
            throw new NotImplementedException();
        }

        private async Task<Guid> GetUserIdFromTokenAsync(string token)
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
