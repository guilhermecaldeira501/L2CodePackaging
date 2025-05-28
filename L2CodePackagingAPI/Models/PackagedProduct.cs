namespace L2CodePackagingAPI.Models
{
    public class PackagedProduct
    {
        public int Id { get; set; }
        public int PackagingResultId { get; set; }
        public int ProductId { get; set; }

        // Navigation properties
        public PackagingResult? PackagingResult { get; set; }
        public Product? Product { get; set; }
    }
}
