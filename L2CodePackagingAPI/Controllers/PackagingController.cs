using L2CodePackagingAPI.DTOs;
using L2CodePackagingAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace L2CodePackagingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagingController : ControllerBase
    {
        private readonly IPackagingService _packagingService;
        private readonly ILogger<PackagingController> _logger;

        public PackagingController(IPackagingService packagingService, ILogger<PackagingController> logger)
        {
            _packagingService = packagingService;
            _logger = logger;
        }

        /// <summary>
        /// Processa pedidos e retorna a melhor forma de embalagem
        /// </summary>
        /// <param name="request">Lista de pedidos com produtos e dimensões</param>
        /// <returns>Resultado da embalagem otimizada</returns>
        [HttpPost("process")]
        public async Task<ActionResult<PackagingResponseDto>> ProcessPackaging([FromBody] PackagingRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!request.Pedidos.Any())
                {
                    return BadRequest("Pelo menos um pedido deve ser fornecido.");
                }

                foreach (var pedido in request.Pedidos)
                {
                    if (!pedido.Produtos.Any())
                    {
                        return BadRequest($"Pedido {pedido.Id} deve conter pelo menos um produto.");
                    }
                }

                _logger.LogInformation($"Processando {request.Pedidos.Count} pedidos com total de {request.Pedidos.Sum(p => p.Produtos.Count)} produtos");

                var result = await _packagingService.ProcessPackagingAsync(request);

                _logger.LogInformation($"Processamento concluído. Total de caixas utilizadas: {result.Pedidos.Sum(p => p.Caixas.Count)}");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar embalagem");
                return StatusCode(500, "Erro interno do servidor ao processar a embalagem.");
            }
        }

        /// <summary>
        /// Endpoint de teste para verificar se a API está funcionando
        /// </summary>
        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok(new
            {
                Status = "Healthy",
                Message = "API de Embalagem do Seu Manoel está funcionando!",
                Timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Retorna informações sobre as caixas disponíveis
        /// </summary>
        [HttpGet("boxes")]
        public async Task<ActionResult> GetAvailableBoxes([FromServices] IBoxService boxService)
        {
            try
            {
                var boxes = await boxService.GetAvailableBoxesAsync();
                var response = boxes.Select(b => new
                {
                    Id = b.Id,
                    Nome = b.Name,
                    Dimensoes = new
                    {
                        Altura = b.Height,
                        Largura = b.Width,
                        Comprimento = b.Length
                    },
                    Volume = b.Volume
                });

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar caixas disponíveis");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }
    }
}
