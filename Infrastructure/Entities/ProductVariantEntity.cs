using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

[PrimaryKey("ProductVariantId")]
public class ProductVariantEntity
{
    [Key]
    public Guid ProductVariantId { get; set; } = Guid.NewGuid();
    [ForeignKey("ArticleNumber")]
    public Guid ArticleNumber { get; set; }
    public virtual ProductEntity Product { get; set; } = null!;
    [ForeignKey("SizeId")]
    public Guid SizeId { get; set; }
    public virtual SizesEntity Size { get; set; } = null!;
    [ForeignKey("ColorId")]
    public Guid ColorId { get; set; }
    public virtual ColorEntity Color { get; set; } = null!;
}
