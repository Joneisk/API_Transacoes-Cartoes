namespace APICARTOES.Models
{
    public class Transacao
    {
        public int TransacaoId { get; set; }
        public decimal Valor {  get; set; }
        public string? Cartao { get; set; }
        public string? CVV { get; set; }
        public int Parcelas { get; set; }
        public int Situacao { get; set; }

    }
}
