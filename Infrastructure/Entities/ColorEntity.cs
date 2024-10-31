using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities;

[PrimaryKey("ColorId")]
public class ColorEntity
{
    [Key]
    public Guid ColorId { get; set; } = Guid.NewGuid();
    public string ColorName { get; set; } = null!;
}
