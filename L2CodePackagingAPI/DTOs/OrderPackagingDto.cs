namespace L2CodePackagingAPI.DTOs
{
    public class OrderPackagingDto
    {
        public string Id { get; set; } = string.Empty;
        public List<BoxPackagingDto> Caixas { get; set; } = new List<BoxPackagingDto>();
    }
}
