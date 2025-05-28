using L2CodePackagingAPI.DTOs;

namespace L2CodePackagingAPI.Services
{
    public interface IPackagingService
    {
        Task<PackagingResponseDto> ProcessPackagingAsync(PackagingRequestDto request);
    }

}
