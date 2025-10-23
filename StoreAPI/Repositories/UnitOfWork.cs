using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using StoreAPI.Context;
using StoreAPI.Interfaces;

namespace StoreAPI.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public readonly AppDbContext _context = context;
    private IDbContextTransaction? _transaction;
    private IProductRepository? _productRepository;
    private IClientRepository? _clientRepository;
    private IRoleRepository? _roleRepository;
    private IOrderRepository? _orderRepository;
    private ICategoryRepository? _categoryRepository;

    public IProductRepository ProductRepository
    {
        get { return _productRepository ??= new ProductRepository(_context); }
    }

    public IClientRepository ClientRepository
    {
        get { return _clientRepository ??= new ClientRepository(_context); }
    }

    public IRoleRepository RoleRepository
    {
        get { return _roleRepository ??= new RoleRepository(_context); }
    }

    public IOrderRepository OrderRepository
    {
        get { return _orderRepository ??= new OrderRepository(_context); }
    }

    public ICategoryRepository CategoryRepository
    {
        get { return _categoryRepository ??= new CategoryRepository(_context); }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task BeginTransaction()
    {
        if (_context.Database.IsInMemory())
            return;

        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        if (_transaction is not null)
            await _transaction.CommitAsync();
    }

    public async Task RollbackAsync()
    {
        if (_context.Database.IsInMemory())
            return;

        if (_transaction is not null)
            await _transaction.RollbackAsync();
    }

    public void Dispose()
    {
        _transaction?.Dispose();
    }
}