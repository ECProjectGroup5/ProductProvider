
namespace Infrastructure.Models;

public class UpdateProductModel
{
    public Guid ArticleNumber { get; set; }
    public string Title { get; set; } = null!;
    public string? Ingress { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public decimal DiscountPrice { get; set; } = 0;
    public string? Manufacturer { get; set; }
    public string? PrimaryImage { get; set; }
}
