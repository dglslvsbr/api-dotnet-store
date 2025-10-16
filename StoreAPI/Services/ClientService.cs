using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using StoreAPI.DTOs;
using StoreAPI.Entities.Authentication;
using StoreAPI.Entities.Models;
using StoreAPI.Enums;
using StoreAPI.Extensions;
using StoreAPI.Interfaces;
using StoreAPI.Useful;

namespace StoreAPI.Services;

public class ClientService(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache memoryCache) : IClientService
{
    public static string ClientListKey => "CacheClient";
    public static string ClientByIdKey(int id) => $"{ClientListKey}/{id}";
    public static string ClientByEmailKey(string email) => $"{ClientListKey}/{email}";

    public async Task<IEnumerable<ShowClientDTO>> GetAllAsync()
    {
        var clientList = await memoryCache.GetOrSetAsync(ClientListKey, () => unitOfWork.ClientRepository.GetAllAsync());
        return mapper.Map<IEnumerable<ShowClientDTO>>(clientList);
    }

    public async Task<ShowClientDTO> GetAsync(int id)
    {
        var client = await memoryCache.GetOrSetAsync(ClientByIdKey(id), () => unitOfWork.ClientRepository.GetAsync(id));
        return mapper.Map<ShowClientDTO>(client);
    }

    public async Task<ShowClientDTO> GetByEmailAsync(string email)
    {
        var client = await memoryCache.GetOrSetAsync(ClientByEmailKey(email), () => unitOfWork.ClientRepository.GetByEmailAsync(email));
        return mapper.Map<ShowClientDTO>(client);
    }

    public async Task<List<DuplicateField>> CheckDuplicates(CreateClientDTO client)
    {
        return await unitOfWork.ClientRepository.CheckDuplicates(client);
    }

    public async Task CreateAsync(CreateClientDTO entity)
    {
        entity.Password = BCrypt.Net.BCrypt.HashPassword(entity.Password);
        entity.CreditCard = GenerateCreditCard.Generate();
        entity.ClientRole = [new ClientRole { RoleId = 1 }];
        await unitOfWork.ClientRepository.CreateAsync(mapper.Map<Client>(entity));
        await unitOfWork.CommitAsync();

        memoryCache.Remove(ClientListKey);
    }

    public async Task UpdateAsync(UpdateClientDTO entity)
    {
        var client = mapper.Map<Client>(entity);
        unitOfWork.ClientRepository.Update(client);
        await unitOfWork.CommitAsync();

        RemoveAllCache(client.Id, entity.Email!);
    }

    public async Task DeleteAsync(ShowClientDTO entity)
    {
        var client = mapper.Map<Client>(entity);
        unitOfWork.ClientRepository.Delete(client);
        await unitOfWork.CommitAsync();

        RemoveAllCache(client.Id, entity.Email!);
    }

    private void RemoveAllCache(int id, string email)
    {
        memoryCache.Remove(ClientListKey);
        memoryCache.Remove(ClientByIdKey(id));
        memoryCache.Remove(ClientByEmailKey(email));
    }
}