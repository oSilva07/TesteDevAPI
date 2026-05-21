using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MinhaApiComSQLite.Models;

namespace MinhaApiComSQLite.Repositories
{
    public interface IProdutoRepository
    {
        Task<IEnumerable<Produto>> ObterTodosAsync();
        Task<Produto> ObterPorIdAsync(int id);
        Task<Produto> ObterPorNomeAsync(string nome);
        Task<Produto> CriarAsync(Produto produto);
        Task AtualizarAsync(Produto produto);
        Task DeletarAsync(int id);
    }
}