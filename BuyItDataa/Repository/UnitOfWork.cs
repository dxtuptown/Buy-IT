using BuyItData.Data;
using BuyItData.Models;
using BuyItData.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyItData.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyDbContext _context;

        public IProductRepository Products { get; private set; }
        public ICategoryRepository Categories { get; private set; }
        public ICompanyRepository Companies { get; private set; }
        public IShoppingCartRepository Carts { get; private set; }
        public IUserRepository Users { get; private set; }
        public IOrderHeaderRepository OrderHeaders { get; private set; }
        public IOrderDetailRepository OrderDetails { get; private set; }

        public UnitOfWork(MyDbContext context)
        {
            _context = context;
            Products = new ProductRepository(_context);
            Categories = new CategoryRepository(_context);
            Companies = new CompanyRepository(_context);
            Carts = new ShoppingCartRepository(_context);
            Users = new UserRepository(_context);
            OrderDetails = new OrderDetailRepository(_context);
            OrderHeaders = new OrderHeaderRepository(_context);
        }

        public async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
