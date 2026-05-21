using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MinhaApiComSQLite.Models;

namespace MinhaApiComSQLite.Repositories
{
    public interface ICategoriaRepository
    {
        Task<IEnumerable<Categoria>> ObterTodasAsync();
        Task<Categoria> ObterPorIdAsync(int id);
        Task<Categoria> ObterPorNomeAsync(string nome);
        Task<Categoria> CriarAsync(Categoria categoria);
        Task AtualizarAsync(Categoria categoria);
        Task DeletarAsync(int id);
    }
}