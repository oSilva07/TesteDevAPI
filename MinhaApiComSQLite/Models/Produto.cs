namespace MinhaApiComSQLite.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public double Preco { get; set; }
        
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }
    }
}
