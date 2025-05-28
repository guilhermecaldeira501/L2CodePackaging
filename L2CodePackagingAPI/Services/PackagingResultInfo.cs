using L2CodePackagingAPI.DTOs;
using L2CodePackagingAPI.Models;

namespace L2CodePackagingAPI.Services
{
    public class PackagingResultInfo
    {
        public Box Box { get; set; } = new Box();
        public List<ProductDto> Products { get; set; } = new List<ProductDto>();
    }
}
