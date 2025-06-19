public class Warehouse
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public string? Location { get; set; }

    public ICollection<ProductStock> ProductStocks { get; set; } = new List<ProductStock>();
}
