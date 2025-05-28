using L2CodePackagingAPI.DTOs;
using L2CodePackagingAPI.Models;

namespace L2CodePackagingAPI.Services
{
    public interface IBoxService
    {
        Task<List<Box>> GetAvailableBoxesAsync();
        Box? FindBestBox(List<ProductDto> products);
    }
}
