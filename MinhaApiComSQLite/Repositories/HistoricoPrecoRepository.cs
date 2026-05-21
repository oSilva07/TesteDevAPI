using Microsoft.EntityFrameworkCore;
using MinhaApiComSQLite.Data;
using MinhaApiComSQLite.Models;

namespace MinhaApiComSQLite.Repositories
{
    public class HistoricoPrecoRepository : IHistoricoPrecoRepository
    {
        private readonly AppDbContext _context;

        public HistoricoPrecoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task SalvarAsync(HistoricoPreco historico)
        {
            await _context.HistoricoPrecos.AddAsync(historico);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<HistoricoPreco>> ObterPorProdutoIdAsync(int produtoId)
        {
            return await _context.HistoricoPrecos
                .Where(h => h.ProdutoId == produtoId)
                .OrderByDescending(h => h.DataAlteracao) 
                .ToListAsync();
        }
    }
}