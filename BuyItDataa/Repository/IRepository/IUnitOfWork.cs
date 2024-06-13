using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuyItData.Models;

namespace BuyItData.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        ICompanyRepository Companies { get; }
        IShoppingCartRepository Carts { get; }
        IUserRepository Users { get; }
        IOrderDetailRepository OrderDetails { get; }
        IOrderHeaderRepository OrderHeaders { get; }
        Task<int> Save();

    }
}
