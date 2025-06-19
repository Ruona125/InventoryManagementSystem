public class PurchaseOrder
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SupplierId { get; set; } = Guid.NewGuid();
    public Guid OrderedBy { get; set; } = Guid.NewGuid();
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "Pending";

    public Supplier? Supplier { get; set; }
    public User? User { get; set; }

    public ICollection<PurchaseOrderItem> Items { get; set; } = new List<PurchaseOrderItem>();
}
