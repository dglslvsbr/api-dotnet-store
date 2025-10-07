namespace StoreAPI.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IProductRepository ProductRepository { get; }
    IClientRepository ClientRepository { get; }
    IRoleRepository RoleRepository { get; }
    IOrderRepository OrderRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    Task BeginTransaction();
    Task CommitAsync();
    Task RollbackAsync();
}