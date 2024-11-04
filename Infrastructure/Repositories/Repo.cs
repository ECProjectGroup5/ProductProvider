using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Factories;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class Repo(DataContext context, ILogger<Repo> logger)
{
    private readonly DataContext _context = context;
    private readonly ILogger<Repo> _logger = logger;

    public async Task<ResponseResult> CreateAsync(ProductEntity product)
    {
        try
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return ResponseFactory.Ok(product);
        }
        catch (Exception ex)
        {
            _logger.LogDebug($"Error: ProductProvider.Repo.CreateAsync {ex.Message}");
            return ResponseFactory.Error();
        }
    }

    public async Task<ResponseResult> GetOneAsync(Expression<Func<ProductEntity, bool>> predicate)
    {
        try
        {
            var product = await _context.Products.FirstOrDefaultAsync(predicate);

            if (product == null)
            {
                return ResponseFactory.NotFound();
            }

            return ResponseFactory.Ok(product);
        }
        catch (Exception ex)
        {
            _logger.LogDebug($"Error: ProductProvider.Repo.GetOneAsync {ex.Message}");
            return ResponseFactory.Error();
        }
    }

    public async Task<ResponseResult> GetAllAsync()
    {
        try
        {
            IEnumerable<ProductEntity> products = await _context.Products.ToListAsync();

            if (!products.Any())
            {
                return ResponseFactory.NotFound();
            }

            return ResponseFactory.Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogDebug($"Error: ProductProvider.Repo.GetAllAsync {ex.Message}");
            return ResponseFactory.Error();
        }
    }

    public async Task<ResponseResult> ExistsAsync(Expression<Func<ProductEntity, bool>> predicate)
    {
        try
        {
            bool exists = await _context.Products.AnyAsync(predicate);

            if (!exists)
            {
                return ResponseFactory.NotFound();
            }

            return ResponseFactory.Exists(exists);
        }
        catch (Exception ex)
        {
            _logger.LogDebug($"Error: ProductProvider.Repo.ExistsAsync {ex.Message}");
            return ResponseFactory.Error();
        }
    }

    public async Task<ResponseResult> UpdateAsync(Expression<Func<ProductEntity, bool>> predicate, ProductEntity product)
    {

        try
        {
            var result = await ExistsAsync(predicate);

            if (result.StatusCode == StatusCode.EXISTS)
            {
                _context.Products.Update(product);
                await _context.SaveChangesAsync();

                return ResponseFactory.Ok(product);
            }

            if (result.StatusCode == StatusCode.NOT_FOUND)
            {
                return ResponseFactory.NotFound();
            }

            return ResponseFactory.Error();
        }
        catch (Exception ex)
        {
            _logger.LogDebug($"Error: ProductProvider.Repo.UpdateAsync {ex.Message}");
            return ResponseFactory.Error();
        }
    }

    public async Task<ResponseResult> DeleteAsync(Expression<Func<ProductEntity, bool>> predicate)
    {
        try
        {
            var result = await ExistsAsync(predicate);

            if (result.StatusCode == StatusCode.EXISTS)
            {
                var product = await _context.Products.FirstOrDefaultAsync(predicate);
                _context.Products.Remove(product!);
                await _context.SaveChangesAsync();

                return ResponseFactory.Ok();
            }

            if (result.StatusCode == StatusCode.NOT_FOUND)
            {
                return ResponseFactory.NotFound();
            }

            return ResponseFactory.Error();
        }
        catch (Exception ex)
        {
            _logger.LogDebug($"Error: ProductProvider.Repo.DeleteAsync {ex.Message}");
            return ResponseFactory.Error();
        }
    }
}
