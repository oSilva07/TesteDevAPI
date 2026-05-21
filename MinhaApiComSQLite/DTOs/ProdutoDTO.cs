namespace MinhaApiComSQLite.DTOs
{
    public class ProdutoDTO
    {
        public string Nome { get; set; } = string.Empty;
        public double Preco { get; set; }
        public int Estoque { get; set; }
        public int CategoriaId { get; set; }
    }
}