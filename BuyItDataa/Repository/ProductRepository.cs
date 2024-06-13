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
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly MyDbContext _context;

        public ProductRepository(MyDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
