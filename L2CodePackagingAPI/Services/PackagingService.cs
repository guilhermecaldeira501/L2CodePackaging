using L2CodePackagingAPI.DTOs;
using L2CodePackagingAPI.Models;

namespace L2CodePackagingAPI.Services
{
    public class PackagingService : IPackagingService
    {
        private readonly IBoxService _boxService;
        private readonly IOrderService _orderService;

        public PackagingService(IBoxService boxService, IOrderService orderService)
        {
            _boxService = boxService;
            _orderService = orderService;
        }

        public async Task<PackagingResponseDto> ProcessPackagingAsync(PackagingRequestDto request)
        {
            var response = new PackagingResponseDto();
            var availableBoxes = await _boxService.GetAvailableBoxesAsync();

            foreach (var orderDto in request.Pedidos)
            {
                var order = await _orderService.CreateOrderAsync(orderDto);
                var packagingResults = OptimizePackaging(orderDto.Produtos, availableBoxes);

                await _orderService.SavePackagingResultAsync(order, packagingResults);

                var orderPackaging = new OrderPackagingDto
                {
                    Id = orderDto.Id,
                    Caixas = packagingResults.Select((result, index) => new BoxPackagingDto
                    {
                        Id = $"{result.Box.Name}_{index + 1}",
                        Produtos = result.Products.Select(p => p.Id).ToList(),
                        Dimensoes = new DimensionsDto
                        {
                            Altura = result.Box.Height,
                            Largura = result.Box.Width,
                            Comprimento = result.Box.Length
                        },
                        VolumeUtilizado = result.Products.Sum(p => p.Altura * p.Largura * p.Comprimento),
                        VolumeTotal = result.Box.Volume,
                        TaxaOcupacao = Math.Round((double)result.Products.Sum(p => p.Altura * p.Largura * p.Comprimento) / result.Box.Volume * 100, 2)
                    }).ToList()
                };

                response.Pedidos.Add(orderPackaging);
            }

            return response;
        }

        private List<PackagingResultInfo> OptimizePackaging(List<ProductDto> products, List<Box> availableBoxes)
        {
            var results = new List<PackagingResultInfo>();
            var remainingProducts = new List<ProductDto>(products);

            // Ordenar produtos por volume (maior primeiro) para melhor otimização
            remainingProducts = remainingProducts.OrderByDescending(p => p.Altura * p.Largura * p.Comprimento).ToList();

            while (remainingProducts.Any())
            {
                var bestFit = FindBestFitForProducts(remainingProducts, availableBoxes);

                if (bestFit != null)
                {
                    results.Add(bestFit);

                    // Remover produtos empacotados da lista
                    foreach (var product in bestFit.Products)
                    {
                        remainingProducts.RemoveAll(p => p.Id == product.Id);
                    }
                }
                else
                {
                    // Se não conseguir empacotar, usar a maior caixa disponível para o maior produto
                    var largestBox = availableBoxes.OrderByDescending(b => b.Volume).First();
                    var largestProduct = remainingProducts.First();

                    results.Add(new PackagingResultInfo
                    {
                        Box = largestBox,
                        Products = new List<ProductDto> { largestProduct }
                    });

                    remainingProducts.Remove(largestProduct);
                }
            }

            return results;
        }

        private PackagingResultInfo? FindBestFitForProducts(List<ProductDto> products, List<Box> availableBoxes)
        {
            PackagingResultInfo? bestFit = null;
            double bestEfficiency = 0;

            foreach (var box in availableBoxes.OrderBy(b => b.Volume))
            {
                var fittingProducts = new List<ProductDto>();
                int usedVolume = 0;

                foreach (var product in products)
                {
                    if (CanFitInBox(product, box) &&
                        (usedVolume + product.Altura * product.Largura * product.Comprimento) <= box.Volume)
                    {
                        fittingProducts.Add(product);
                        usedVolume += product.Altura * product.Largura * product.Comprimento;
                    }
                }

                if (fittingProducts.Any())
                {
                    double efficiency = (double)usedVolume / box.Volume;

                    if (efficiency > bestEfficiency)
                    {
                        bestEfficiency = efficiency;
                        bestFit = new PackagingResultInfo
                        {
                            Box = box,
                            Products = fittingProducts
                        };
                    }
                }
            }

            return bestFit;
        }

        private bool CanFitInBox(ProductDto product, Box box)
        {
            // Verifica se o produto cabe na caixa considerando rotações possíveis
            var productDimensions = new[] { product.Altura, product.Largura, product.Comprimento };
            var boxDimensions = new[] { box.Height, box.Width, box.Length };

            Array.Sort(productDimensions);
            Array.Sort(boxDimensions);

            return productDimensions[0] <= boxDimensions[0] &&
                   productDimensions[1] <= boxDimensions[1] &&
                   productDimensions[2] <= boxDimensions[2];
        }
    }
}
