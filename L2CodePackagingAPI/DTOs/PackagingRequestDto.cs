using System.ComponentModel.DataAnnotations;

namespace L2CodePackagingAPI.DTOs
{
    public class PackagingRequestDto
    {
        [Required]
        public List<OrderDto> Pedidos { get; set; } = new List<OrderDto>();
    }
}
