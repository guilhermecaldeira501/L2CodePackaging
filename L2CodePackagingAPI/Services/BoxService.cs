using L2CodePackagingAPI.Data;
using L2CodePackagingAPI.DTOs;
using L2CodePackagingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace L2CodePackagingAPI.Services
{
    public class BoxService : IBoxService
    {
        private readonly PackagingDbContext _context;

        public BoxService(PackagingDbContext context)
        {
            _context = context;
        }

        public async Task<List<Box>> GetAvailableBoxesAsync()
        {
            return await _context.Boxes.OrderBy(b => b.Height * b.Width * b.Length).ToListAsync();
        }

        public Box? FindBestBox(List<ProductDto> products)
        {
            var totalVolume = products.Sum(p => p.Altura * p.Largura * p.Comprimento);
            var availableBoxes = _context.Boxes.Where(b => b.Volume >= totalVolume).OrderBy(b => b.Volume).ToList();

            return availableBoxes.FirstOrDefault();
        }
    }
}
