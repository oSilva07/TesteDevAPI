using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MinhaApiComSQLite.DTOs;
using MinhaApiComSQLite.Models;

namespace MinhaApiComSQLite.Services
{
    public interface ICategoriaService
    {
        Task<IEnumerable<Categoria>> ObterTodasAsync();
        Task<Categoria> ObterPorIdAsync(int id);
        Task<Categoria> CriarAsync(CategoriaDTO dto);
        Task DeletarAsync(int id);
    }
}