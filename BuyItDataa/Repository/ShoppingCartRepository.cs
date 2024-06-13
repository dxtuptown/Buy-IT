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
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly MyDbContext _context;

        public ShoppingCartRepository(MyDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<int> DecrementCount(ShoppingCart shoppingCart, int count)
        {
            shoppingCart.Count -= count;
            return shoppingCart.Count;
        }

        public async Task<int> IncrementCount(ShoppingCart shoppingCart, int count)
        {
            if (shoppingCart == null)
            {
                throw new ArgumentNullException(nameof(shoppingCart));
            }
            shoppingCart.Count += count;
            return shoppingCart.Count;
        }
    }
}
