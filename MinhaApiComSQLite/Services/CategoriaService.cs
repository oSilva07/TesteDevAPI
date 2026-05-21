using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MinhaApiComSQLite.DTOs;
using MinhaApiComSQLite.Models;
using MinhaApiComSQLite.Repositories;

namespace MinhaApiComSQLite.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _repository;

        public CategoriaService(ICategoriaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Categoria>> ObterTodasAsync() => await _repository.ObterTodasAsync();

        public async Task<Categoria> ObterPorIdAsync(int id) => await _repository.ObterPorIdAsync(id);

        public async Task<Categoria> CriarAsync(CategoriaDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nome)) throw new ArgumentException("O nome da categoria é obrigatório.");

            string nomeFormatado = char.ToUpper(dto.Nome.Trim()[0]) + dto.Nome.Trim().Substring(1);

            var categoriaExistente = await _repository.ObterPorNomeAsync(nomeFormatado);
            if (categoriaExistente != null) throw new InvalidOperationException("Já existe uma categoria com este nome.");

            var novaCategoria = new Categoria { Nome = nomeFormatado };
            return await _repository.CriarAsync(novaCategoria);
        }

        public async Task DeletarAsync(int id) => await _repository.DeletarAsync(id);
    }
}