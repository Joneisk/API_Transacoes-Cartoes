using APICARTOES.Repository;

namespace APICARTOES.Services
{
    public class CartaoService
    {
        private readonly CartaoRepository _cartaoRepository;

        public CartaoService(CartaoRepository cartaoRepository)
        {
            _cartaoRepository = cartaoRepository;
        }

        public string ObterPorNumero(string cartao)
        {
            if (string.IsNullOrWhiteSpace(cartao) || cartao.Length < 9)
                return "BANDEIRA DESCONHECIDA";

            var sucesso = _cartaoRepository.ObterPorNumero(cartao);
            if (!sucesso)
                return "BANDEIRA DESCONHECIDA";

            string tipoCartao;

            if (cartao[0] == '1' && cartao[8] == '1')
                tipoCartao = "VISA";
            else if (cartao[0] == '2' && cartao[8] == '2')
                tipoCartao = "MASTERCARD";
            else if (cartao[0] == '3' && cartao[8] == '3')
                tipoCartao = "ELO";
            else
                tipoCartao = "BANDEIRA DESCONHECIDA";

            return tipoCartao;
        }


        public bool ObterCartaoValido(string cartao)
        {

            var sucesso = _cartaoRepository.ObterCartaoValido(cartao);

            return sucesso;
        }

    }
}
