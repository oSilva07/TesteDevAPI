using MinhaApiComSQLite.Models;

namespace MinhaApiComSQLite.Repositories
{
    public interface IHistoricoPrecoRepository
    {
        Task SalvarAsync(HistoricoPreco historico);
        Task<IEnumerable<HistoricoPreco>> ObterPorProdutoIdAsync(int produtoId);
    }
}