using Infrastructure.Entities;
using Infrastructure.Models;
using System.Linq.Expressions;

namespace Infrastructure.Interfaces
{
    public interface IRepo
    {
        Task<ResponseResult> CreateAsync(ProductEntity product);
        Task<ResponseResult> DeleteAsync(Expression<Func<ProductEntity, bool>> predicate);
        Task<ResponseResult> ExistsAsync(Expression<Func<ProductEntity, bool>> predicate);
        Task<ResponseResult> GetAllAsync();
        Task<ResponseResult> GetOneAsync(Expression<Func<ProductEntity, bool>> predicate);
        Task<ResponseResult> UpdateAsync(Expression<Func<ProductEntity, bool>> predicate, ProductEntity product);
    }
}