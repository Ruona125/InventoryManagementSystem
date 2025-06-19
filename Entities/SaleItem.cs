public class SaleItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SaleId { get; set; } = Guid.NewGuid();
    public Guid ProductId { get; set; } = Guid.NewGuid();
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public Sale? Sale { get; set; }
    public Product? Product { get; set; }
}
