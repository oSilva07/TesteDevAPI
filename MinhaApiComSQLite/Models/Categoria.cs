namespace MinhaApiComSQLite.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;

        // Propriedade para o Entity Framework entender o relacionamento de 1 para Muitos
        public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
    }
}
