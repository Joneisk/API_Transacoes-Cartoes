namespace APICARTOES.DTOs
{
    public class CadastroTransacaoDTO
    {
        public decimal ValorTotal { get; set; }
        public decimal TaxaDeJuros { get; set; }
        public int QtdeParcelas { get; set; }
    }
}
