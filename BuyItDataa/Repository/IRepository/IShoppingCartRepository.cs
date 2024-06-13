using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuyItData.Models;

namespace BuyItData.Repository.IRepository
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        public Task<int> IncrementCount(ShoppingCart shoppingCart, int count);
        public Task<int> DecrementCount(ShoppingCart shoppingCart, int count);
    }
}