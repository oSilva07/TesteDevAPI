namespace MinhaApiComSQLite.DTOs
{
    public class SimulacaoDescontoDTO
    {
        public int ProdutoId { get; set; }
        public string ProdutoNome { get; set; }
        public double PrecoUnitario { get; set; }
        public int Quantidade { get; set; }
        public double PrecoTotalSemDesconto { get; set; }
        public double PercentualDescontoAplicado { get; set; }
        public double ValorDesconto { get; set; }
        public double PrecoTotalComDesconto { get; set; }
    }
}