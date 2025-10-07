using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using StoreAPI.DTOs;
using StoreAPI.Entities.Authentication;
using StoreAPI.Extensions;
using StoreAPI.Interfaces;

namespace StoreAPI.Services;

public class RoleService(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache memoryCache) : IRoleService
{
    private static string RoleListKey => "CacheRole";
    private static string RoleByIdKey(int id) => $"{RoleListKey}/{id}";
    private static string RoleByNameKey(string name) => $"{RoleListKey}/{name}";

    public async Task<IEnumerable<CreateRoleDTO>> GetAllAsync()
    {
        var roleList = await memoryCache.GetOrSetAsync(RoleListKey, () => unitOfWork.RoleRepository.GetAllAsync());
        return mapper.Map<IEnumerable<CreateRoleDTO>>(roleList);
    }

    public async Task<CreateRoleDTO> GetAsync(int id)
    {
        var role = await memoryCache.GetOrSetAsync(RoleByIdKey(id), () => unitOfWork.RoleRepository.GetAsync(id));
        return mapper.Map<CreateRoleDTO>(role);
    }

    public async Task<CreateRoleDTO> GetByNameAsync(string name)
    {
        var role = await memoryCache.GetOrSetAsync(RoleByNameKey(name), () => unitOfWork.RoleRepository.GetByNameAsync(name));
        return mapper.Map<CreateRoleDTO>(role);
    }

    public async Task CreateAsync(Role entity)
    {
        await unitOfWork.RoleRepository.CreateAsync(entity);
        await unitOfWork.CommitAsync();

        memoryCache.Remove(RoleListKey);
    }

    public async Task UpdateAsync(Role entity)
    {
        unitOfWork.RoleRepository.Update(entity);
        await unitOfWork.CommitAsync();

        RemoveAllCache(entity.Id, entity.Name!);
    }

    public async Task DeleteAsync(Role entity)
    {
        unitOfWork.RoleRepository.Delete(entity);
        await unitOfWork.CommitAsync();

        RemoveAllCache(entity.Id, entity.Name!);
    }

    private void RemoveAllCache(int id, string name)
    {
        memoryCache.Remove(RoleListKey);
        memoryCache.Remove(RoleByIdKey(id));
        memoryCache.Remove(RoleByNameKey(name));
    }
}