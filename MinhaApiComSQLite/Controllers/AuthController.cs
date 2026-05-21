using Microsoft.AspNetCore.Mvc;
using MinhaApiComSQLite.DTOs;
using MinhaApiComSQLite.Services;

namespace MinhaApiComSQLite.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;

        public AuthController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO dto)
        {
            // Validação simples mockada para o teste
            if (dto.Usuario == "admin" && dto.Senha == "admin123")
            {
                var token = _tokenService.GerarToken(dto.Usuario);
                return Ok(new { token = token });
            }

            return Unauthorized("Usuário ou senha inválidos.");
        }
    }
}