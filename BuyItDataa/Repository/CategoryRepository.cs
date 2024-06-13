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
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly MyDbContext _context;

        public CategoryRepository(MyDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
