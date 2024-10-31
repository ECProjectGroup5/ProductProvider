using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

[PrimaryKey("ProductVariantId")]
public class StockEntity
{
    [Key]
    public Guid ProductVariantId { get; set; }
    [ForeignKey("ProductVariantId")]
    public virtual ProductVariantEntity ProductVariant { get; set; } = null!;
    public int Stock { get; set; }
}