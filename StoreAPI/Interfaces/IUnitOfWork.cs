using StoreAPI.Repositories;

namespace StoreAPI.Interfaces
{
    public interface IUnitOfWork
    {
        CategoryRepository CategoryRepository { get; }
        ProductRepository ProductRepository { get; }
        ClientRepository ClientRepository { get; }
        RoleRepository RoleRepository { get; }
        OrderRepository OrderRepository { get; }
        OrderItemRepository OrderItemRepository { get; }
        Task CommitAsync();
        Task RollbackAsync();
    }
}