using Microsoft.AspNetCore.Mvc;
using MinhaApiComSQLite.DTOs;
using MinhaApiComSQLite.Services;
using Microsoft.AspNetCore.Authorization;

namespace MinhaApiComSQLite.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class ProdutoController : ControllerBase
    {
        private readonly ILogger<ProdutoController> _logger;
        private readonly IProdutoService _service;

        public ProdutoController(ILogger<ProdutoController> logger, IProdutoService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            _logger.LogInformation($"Buscando produtos - Página: {pageNumber}, Tamanho: {pageSize}");
            
            var products = await _service.ObterTodosAsync();

            var produtosPaginados = products
                .OrderBy(p => p.Nome)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return Ok(produtosPaginados);
        }

        [HttpGet("{id:int}")] // Restringe para aceitar apenas números, evitando conflito com rotas de texto
        public async Task<IActionResult> GetPorId(int id)
        {
            var produto = await _service.ObterPorIdAsync(id);
            if (produto == null)
            {
                _logger.LogWarning($"Produto com ID {id} não foi encontrado.");
                return NotFound("Produto não encontrado.");
            }
            return Ok(produto);
        }

        // 2. Endpoints Protegidos (Exigem Token JWT)

        [HttpGet("relatorio/estatisticas")]
        [Authorize] 
        public async Task<IActionResult> GetEstatisticas()
        {
            _logger.LogInformation("Gerando relatório consolidado e estatísticas de estoque...");
            var relatorio = await _service.ObterRelatorioEstatisticasAsync();
            return Ok(relatorio);
        }

        [HttpGet("{id:int}/historico-precos")]
        [Authorize] 
        public async Task<IActionResult> GetHistorico(int id)
        {
            _logger.LogInformation($"Consultando histórico de preços para o produto ID {id}...");
            
            var produto = await _service.ObterPorIdAsync(id);
            if (produto == null) return NotFound("Produto não encontrado.");

            var historico = await _service.ObterHistoricoPrecosAsync(id);
            return Ok(historico);
        }

        [HttpGet("{id:int}/calcular-desconto")]
        [Authorize] 
        public async Task<IActionResult> CalcularDesconto(int id, [FromQuery] int quantidade)
        {
            try
            {
                var resultado = await _service.CalcularDescontoProgressivoAsync(id, quantidade);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize] 
        public async Task<IActionResult> Post([FromBody] ProdutoDTO dto)
        {
            try
            {
                var novoProduto = await _service.CriarAsync(dto);
                _logger.LogInformation($"Produto '{novoProduto.Nome}' criado com sucesso com o ID {novoProduto.Id}.");
                return CreatedAtAction(nameof(GetPorId), new { id = novoProduto.Id }, novoProduto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao criar produto: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        [Authorize] 
        public async Task<IActionResult> Put(int id, [FromBody] ProdutoDTO dto)
        {
            try
            {
                await _service.AtualizarAsync(id, dto);
                _logger.LogInformation($"Produto ID {id} atualizado com sucesso.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao atualizar produto ID {id}: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize] 
        public async Task<IActionResult> Delete(int id)
        {
            var produto = await _service.ObterPorIdAsync(id);
            if (produto == null) return NotFound("Produto não encontrado.");

            await _service.DeletarAsync(id);
            _logger.LogInformation($"Produto ID {id} deletado.");
            return NoContent();
        }
    }
}