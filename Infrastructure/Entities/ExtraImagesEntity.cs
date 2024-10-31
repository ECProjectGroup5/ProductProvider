using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities;

[PrimaryKey("ExtraImageId")]
public class ExtraImagesEntity
{
    [Key]
    public Guid ExtraImageId { get; set; } = Guid.NewGuid();
    public string ImageUrl { get; set; } = null!;
    public virtual ICollection<ExtraProductImagesEntity>? ExtraProductImages { get; set; }
}