using StoreAPI.AppContext;
using StoreAPI.Interfaces;

namespace StoreAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private CategoryRepository? _categoryRepository;
        private ProductRepository? _productRepository;
        private ClientRepository? _clientRepository;
        private RoleRepository? _roleRepository;
        private OrderRepository? _orderRepository;
        private OrderItemRepository? _orderItemRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public CategoryRepository CategoryRepository
        {
            get { return _categoryRepository ??= new CategoryRepository(_context); }
        }

        public ProductRepository ProductRepository
        {
            get { return _productRepository ??= new ProductRepository(_context); }
        }

        public ClientRepository ClientRepository
        {
            get {  return _clientRepository ??= new ClientRepository(_context); }
        }

        public RoleRepository RoleRepository
        {
            get { return _roleRepository ??= new RoleRepository(_context); }
        }

        public OrderRepository OrderRepository
        {
            get { return _orderRepository ??= new OrderRepository(_context); }
        }

        public OrderItemRepository OrderItemRepository
        {
            get { return _orderItemRepository ??= new OrderItemRepository(_context); }
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task RollbackAsync()
        {
            // Not Implemented
        }
    }
}