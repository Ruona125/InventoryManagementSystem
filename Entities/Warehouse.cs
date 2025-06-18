public class Warehouse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Location { get; set; }

    public ICollection<ProductStock> ProductStocks { get; set; } = new List<ProductStock>();
}
