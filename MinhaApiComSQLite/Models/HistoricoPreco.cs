using System.ComponentModel.DataAnnotations.Schema;

namespace MinhaApiComSQLite.Models
{
    [Table("HistoricoPreco")]
    public class HistoricoPreco
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public double PrecoAnterior { get; set; }
        public double PrecoNovo { get; set; }
        public DateTime DataAlteracao { get; set; }

        public Produto Produto { get; set; }
    }
}