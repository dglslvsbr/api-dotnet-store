using Microsoft.EntityFrameworkCore;

namespace StoreAPI.Services;

public static class PaginatedService
{
    public async static Task<IEnumerable<T>> EntityPaginated<T>(IQueryable<T> source, int pageNumber, int pageSize)
    {
        if (pageSize <= 0)
            throw new ArgumentException("Page size must be greater than zero.");

        if (pageSize > 20)
            throw new ArgumentException("Page size must not be greater than 10.");

        return await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
    }
}