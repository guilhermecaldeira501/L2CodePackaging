namespace L2CodePackagingAPI.DTOs
{
    public class PackagingResponseDto
    {
        public List<OrderPackagingDto> Pedidos { get; set; } = new List<OrderPackagingDto>();
    }
}
