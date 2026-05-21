using MinhaApiComSQLite.DTOs;
using MinhaApiComSQLite.Models;
using MinhaApiComSQLite.Repositories;

namespace MinhaApiComSQLite.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _repository;
        private readonly IHistoricoPrecoRepository _historicoRepository;

        public ProdutoService(IProdutoRepository repository, IHistoricoPrecoRepository historicoRepository)
        {
            _repository = repository;
            _historicoRepository = historicoRepository;
        }

        public async Task<IEnumerable<Produto>> ObterTodosAsync() => await _repository.ObterTodosAsync();

        public async Task<Produto> ObterPorIdAsync(int id) => await _repository.ObterPorIdAsync(id);

        public async Task<Produto> CriarAsync(ProdutoDTO dto)
        {
            if (dto.Preco <= 0) throw new ArgumentException("O preço do produto deve ser maior que zero.");

            string nomeFormatado = FormatarPrimeiraLetraMaior(dto.Nome);

            var produtoExistente = await _repository.ObterPorNomeAsync(nomeFormatado);
            if (produtoExistente != null) throw new InvalidOperationException("Já existe um produto cadastrado com este nome.");

            var novoProduto = new Produto
            {
                Nome = nomeFormatado,
                Preco = dto.Preco,
                Estoque = dto.Estoque,
                CategoriaId = dto.CategoriaId
            };

            var produtoSalvo = await _repository.CriarAsync(novoProduto);

            await _historicoRepository.SalvarAsync(new HistoricoPreco
            {
                ProdutoId = produtoSalvo.Id,
                PrecoAnterior = 0,
                PrecoNovo = produtoSalvo.Preco,
                DataAlteracao = DateTime.Now
            });

            return produtoSalvo;
        }

        public async Task App_UpdateAsync(int id, ProdutoDTO dto) => await AtualizarAsync(id, dto); 

        public async Task AtualizarAsync(int id, ProdutoDTO dto)
        {
            var produto = await _repository.ObterPorIdAsync(id);
            if (produto == null) throw new KeyNotFoundException("Produto não encontrado.");

            if (dto.Preco <= 0) throw new ArgumentException("O preço do produto deve ser maior que zero.");

            if (produto.Preco != dto.Preco)
            {
                await _historicoRepository.SalvarAsync(new HistoricoPreco
                {
                    ProdutoId = produto.Id,
                    PrecoAnterior = produto.Preco,
                    PrecoNovo = dto.Preco,
                    DataAlteracao = DateTime.Now
                });
            }

            string nomeFormatado = FormatarPrimeiraLetraMaior(dto.Nome);
            produto.Nome = nomeFormatado;
            produto.Preco = dto.Preco;
            produto.Estoque = dto.Estoque;
            produto.CategoriaId = dto.CategoriaId;

            await _repository.AtualizarAsync(produto);
        }

        public async Task DeletarAsync(int id) => await _repository.DeletarAsync(id);

        public async Task<IEnumerable<HistoricoPreco>> ObterHistoricoPrecosAsync(int produtoId)
        {
            return await _historicoRepository.ObterPorProdutoIdAsync(produtoId);
        }

        private string FormatarPrimeiraLetraMaior(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return string.Empty;
            texto = texto.Trim();
            return char.ToUpper(texto[0]) + texto.Substring(1);
        }
        public async Task<SimulacaoDescontoDTO> CalcularDescontoProgressivoAsync(int produtoId, int quantidade)
        {
            var produto = await _repository.ObterPorIdAsync(produtoId);
            if (produto == null) throw new KeyNotFoundException("Produto não encontrado.");
            if (quantidade <= 0) throw new ArgumentException("A quantidade deve ser maior que zero.");

            double precoTotalSemDesconto = produto.Preco * quantidade;
            double percentualDesconto = 0;

            if (quantidade >= 5)
            {
                int multiplicador = (int)Math.Floor(Math.Log((double)quantidade / 5, 2));
                
                percentualDesconto = 5 + (multiplicador * 5);

                if (percentualDesconto > 25)
                {
                    percentualDesconto = 25;
                }
            }

            double valorDesconto = precoTotalSemDesconto * (percentualDesconto / 100);
            double precoTotalComDesconto = precoTotalSemDesconto - valorDesconto;

            return new SimulacaoDescontoDTO
            {
                ProdutoId = produto.Id,
                ProdutoNome = produto.Nome,
                PrecoUnitario = produto.Preco,
                Quantidade = quantidade,
                PrecoTotalSemDesconto = precoTotalSemDesconto,
                PercentualDescontoAplicado = percentualDesconto,
                ValorDesconto = valorDesconto,
                PrecoTotalComDesconto = precoTotalComDesconto
            };
        }
        public async Task<RelatorioEstatisticasDTO> ObterRelatorioEstatisticasAsync()
        {
            var produtos = await _repository.ObterTodosAsync();

            if (!produtos.Any())
            {
                return new RelatorioEstatisticasDTO();
            }

            return new RelatorioEstatisticasDTO
            {
                TotalProdutosCadastrados = produtos.Count(),
                MediaPrecos = Math.Round(produtos.Average(p => p.Preco), 2),
                ValorTotalEstoque = Math.Round(produtos.Sum(p => p.Preco * p.Estoque), 2)
            };
        }
    }
}