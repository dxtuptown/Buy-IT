using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BuyItData.Models;

namespace BuyItData.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        public Task<T> GetByIdAsync(Expression <Func<T, bool>> filter, string? includeProperties=null);
        public Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        public Task AddAsync(T entity);
        public Task UpdateAsync(T entity);
        public Task DeleteAsync(T entity);
        public Task<IEnumerable<Product>> GetAllWithCategoryAsync();
		public Task RemoveRange(IEnumerable<T> entity);
	}
}
