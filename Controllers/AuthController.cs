using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjetoESG.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
        /// Gera um token JWT para autenticação básica
        /// </summary>
        /// <param name="request">Credenciais de login</param>
        /// <returns>Token JWT</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), 200)]
        [ProducesResponseType(401)]
        public ActionResult<LoginResponse> Login([FromBody] LoginRequest request)
        {
            // Validação básica (em produção, usar sistema de autenticação real)
            if (request.Username == "admin" && request.Password == "esg123")
            {
                var token = GenerateJwtToken("admin", "Administrator");
                
                _logger.LogInformation("🔐 [AUTH] Login bem-sucedido - Usuário: {Username}", request.Username);
                
                return Ok(new LoginResponse
                {
                    Token = token,
                    ExpiresIn = 3600, // 1 hora
                    TokenType = "Bearer",
                    Username = request.Username
                });
            }

            _logger.LogWarning("❌ [AUTH] Login falhado - Usuário: {Username}", request.Username);
            return Unauthorized(new { message = "Credenciais inválidas" });
        }

        private string GenerateJwtToken(string username, string role)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? "ProjetoESG-SecretKey-2024-MinLength32Characters!";
            var issuer = jwtSettings["Issuer"] ?? "ProjetoESG";
            var audience = jwtSettings["Audience"] ?? "ProjetoESG-Users";

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
        public string TokenType { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
    }
} 