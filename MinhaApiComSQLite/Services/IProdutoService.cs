using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MinhaApiComSQLite.DTOs;
using MinhaApiComSQLite.Models;

namespace MinhaApiComSQLite.Services
{
    public interface IProdutoService
    {
        Task<IEnumerable<Produto>> ObterTodosAsync();
        Task<Produto> ObterPorIdAsync(int id);
        Task<Produto> CriarAsync(ProdutoDTO dto);
        Task AtualizarAsync(int id, ProdutoDTO dto);
        Task DeletarAsync(int id);
        Task<IEnumerable<HistoricoPreco>> ObterHistoricoPrecosAsync(int produtoId);
        Task<SimulacaoDescontoDTO> CalcularDescontoProgressivoAsync(int produtoId, int quantidade);
        Task<RelatorioEstatisticasDTO> ObterRelatorioEstatisticasAsync();
    }
}