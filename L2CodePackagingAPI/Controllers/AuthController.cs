using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using L2CodePackagingAPI.DTOs;
using L2CodePackagingAPI.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace L2CodePackagingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IConfiguration configuration, ILogger<AuthController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Endpoint para autenticação (apenas se JWT estiver configurado)
        /// </summary>
        /// <param name="request">Credenciais de acesso</param>
        /// <returns>Token JWT</returns>
        [HttpPost("login")]
        public IActionResult Login([FromBody] AuthRequestDto request)
        {
            var jwtKey = _configuration["Jwt:Key"];

            if (string.IsNullOrEmpty(jwtKey))
            {
                return BadRequest("Autenticação JWT não está configurada.");
            }

            // Validação simples (em produção, usar sistema de autenticação mais robusto)
            if (request.Username == "admin" && request.Password == "admin123")
            {
                var token = GenerateJwtToken(request.Username);

                return Ok(new AuthResponseDto
                {
                    Token = token,
                    ExpiresAt = DateTime.UtcNow.AddHours(1)
                });
            }

            return Unauthorized("Credenciais inválidas.");
        }

        private string GenerateJwtToken(string username)
        {
            var jwtKey = _configuration["Jwt:Key"];
            var jwtIssuer = _configuration["Jwt:Issuer"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "User")
            };

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtIssuer,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
