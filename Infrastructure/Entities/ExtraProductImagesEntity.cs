using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

[PrimaryKey("ArticleNumber", "ExtraImageId")]
public class ExtraProductImagesEntity
{
    [Key]
    public Guid ArticleNumber { get; set; }
    [ForeignKey("ArticleNumber")]
    public virtual ProductEntity Product { get; set; } = null!;
    [Key]
    public Guid ExtraImageId { get; set; }
    [ForeignKey("ImageId")]
    public virtual ExtraImagesEntity ExtraImages { get; set; } = null!;
}
