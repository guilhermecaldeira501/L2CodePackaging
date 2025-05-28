namespace L2CodePackagingAPI.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public List<Product> Products { get; set; } = new List<Product>();
        public List<PackagingResult> PackagingResults { get; set; } = new List<PackagingResult>();
    }
}
