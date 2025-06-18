public class PurchaseOrder
{
    public int Id { get; set; }
    public int SupplierId { get; set; }
    public int OrderedBy { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "Pending";

    public Supplier? Supplier { get; set; }
    public User? User { get; set; }

    public ICollection<PurchaseOrderItem> Items { get; set; } = new List<PurchaseOrderItem>();
}
