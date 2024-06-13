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
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly MyDbContext _context;

        public CompanyRepository(MyDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
