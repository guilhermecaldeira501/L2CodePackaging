using System.ComponentModel.DataAnnotations;

namespace L2CodePackagingAPI.DTOs
{
    public class ProductDto
    {
        [Required]
        public string Id { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Altura deve ser maior que 0")]
        public int Altura { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Largura deve ser maior que 0")]
        public int Largura { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Comprimento deve ser maior que 0")]
        public int Comprimento { get; set; }
    }
}
