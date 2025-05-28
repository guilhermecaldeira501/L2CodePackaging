using System.ComponentModel.DataAnnotations;

namespace L2CodePackagingAPI.DTOs
{
    public class OrderDto
    {
        [Required]
        public string Id { get; set; } = string.Empty;

        [Required]
        public List<ProductDto> Produtos { get; set; } = new List<ProductDto>();
    }
}
