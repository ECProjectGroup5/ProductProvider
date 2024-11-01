using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public partial class DataContext : DbContext
{
    public DataContext()
    {
    }
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
    public virtual DbSet<SizesEntity> Sizes { get; set; }
    public virtual DbSet<ColorEntity> Colors { get; set; }
    public virtual DbSet<ExtraImagesEntity> ExtraImages { get; set; }
    public virtual DbSet<ProductEntity> Products { get; set; }
    public virtual DbSet<ExtraProductImagesEntity> ExtraProductImages { get; set; }
    public virtual DbSet<ProductVariantEntity> ProductVariants { get; set; }
    public virtual DbSet<StockEntity> Stock { get; set; }
}
