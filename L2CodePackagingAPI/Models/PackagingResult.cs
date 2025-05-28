namespace L2CodePackagingAPI.Models
{
    public class PackagingResult
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int BoxId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Order? Order { get; set; }
        public Box? Box { get; set; }
        public List<PackagedProduct> PackagedProducts { get; set; } = new List<PackagedProduct>();
    }
}
