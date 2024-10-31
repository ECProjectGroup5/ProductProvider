using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities;

[PrimaryKey("SizeId")]
public class SizesEntity
{
    [Key]
    public Guid SizeId { get; set; } = Guid.NewGuid();
    public string SizeName { get; set; } = null!;
}
