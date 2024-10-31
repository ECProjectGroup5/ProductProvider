using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

[PrimaryKey("ArticleNumber")]
public class ProductEntity
{
    [Key]
    public Guid ArticleNumber { get; set; } = Guid.NewGuid();
    [Required]
    [Column(TypeName = "money")]
    public decimal Price { get; set; }
    [Column(TypeName = "money")]
    public decimal DiscountPrice { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? Ingress { get; set; }
    public string? Manufacturer { get; set; }
    public string? PrimaryImage { get; set; }
    public virtual ICollection<ExtraProductImagesEntity>? ExtraProductImages { get; set; }
    public virtual ICollection<ProductVariantEntity>? ProductVariants { get; set; }
}
