using Microsoft.AspNetCore.Mvc;
using MinhaApiComSQLite.DTOs;
using MinhaApiComSQLite.Services;
using Microsoft.AspNetCore.Authorization; 

namespace MinhaApiComSQLite.Controllers
{
    [ApiController] 
    [Route("api/[controller]")] 
    public class CategoriaController : ControllerBase 
    {
        private readonly ILogger<CategoriaController> _logger;
        private readonly ICategoriaService _service;

        public CategoriaController(ILogger<CategoriaController> logger, ICategoriaService service)
        {
            _logger = logger;
            _service = service;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Buscando todas as categorias...");
            var categorias = await _service.ObterTodasAsync();
            return Ok(categorias);
        }

        [HttpGet("{id:int}")] // Restringe apenas inteiros
        public async Task<IActionResult> GetPorId(int id)
        {
            var categoria = await _service.ObterPorIdAsync(id);
            if (categoria == null) 
            {
                _logger.LogWarning($"Categoria com ID {id} não foi encontrada.");
                return NotFound("Categoria não encontrada.");
            }
            return Ok(categoria);
        }


        [HttpPost]
        [Authorize] // Valida token
        public async Task<IActionResult> Post([FromBody] CategoriaDTO dto)
        {
            try
            {
                var novaCategoria = await _service.CriarAsync(dto);
                _logger.LogInformation($"Categoria {novaCategoria.Nome} criada com sucesso!");
                return CreatedAtAction(nameof(GetPorId), new { id = novaCategoria.Id }, novaCategoria);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao criar categoria: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:int}")] // Restringe apenas inteiros
        [Authorize] // Valida token
        public async Task<IActionResult> Delete(int id)
        {
            var categoria = await _service.ObterPorIdAsync(id);
            if (categoria == null) return NotFound("Categoria não encontrada.");

            await _service.DeletarAsync(id);
            _logger.LogInformation($"Categoria ID {id} foi deletada.");
            return NoContent();
        }
    }
}