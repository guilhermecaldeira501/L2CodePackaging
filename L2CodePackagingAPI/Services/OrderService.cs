using L2CodePackagingAPI.Data;
using L2CodePackagingAPI.DTOs;
using L2CodePackagingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace L2CodePackagingAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly PackagingDbContext _context;

        public OrderService(PackagingDbContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateOrderAsync(OrderDto orderDto)
        {
            var existingOrder = await _context.Orders.Include(o => o.Products).FirstOrDefaultAsync(o => o.OrderNumber == orderDto.Id);
            if (existingOrder != null)
            {
                return existingOrder;
            }

            var order = new Order
            {
                OrderNumber = orderDto.Id,
                CreatedAt = DateTime.UtcNow
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Adicionar produtos
            foreach (var productDto in orderDto.Produtos)
            {
                var product = new Product
                {
                    Name = productDto.Id,
                    Height = productDto.Altura,
                    Width = productDto.Largura,
                    Length = productDto.Comprimento,
                    OrderId = order.Id
                };

                _context.Products.Add(product);
            }

            await _context.SaveChangesAsync();
            return order;
        }

        public async Task SavePackagingResultAsync(Order order, List<PackagingResultInfo> results)
        {
            foreach (var result in results)
            {
                var packagingResult = new PackagingResult
                {
                    OrderId = order.Id,
                    BoxId = result.Box.Id,
                    CreatedAt = DateTime.UtcNow
                };

                _context.PackagingResults.Add(packagingResult);
                await _context.SaveChangesAsync();

                // Adicionar produtos empacotados
                foreach (var productDto in result.Products)
                {
                    var product = await _context.Products
                        .FirstOrDefaultAsync(p => p.Name == productDto.Id && p.OrderId == order.Id);

                    if (product != null)
                    {
                        var packagedProduct = new PackagedProduct
                        {
                            PackagingResultId = packagingResult.Id,
                            ProductId = product.Id
                        };

                        _context.PackagedProducts.Add(packagedProduct);
                    }
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
