using APICARTOES.DTOs;
using APICARTOES.Models;
using APICARTOES.Repository;

namespace APICARTOES.Services
{
    public class TransacaoService
    {
        CartaoService cartaoService;
        TransacaoRepository transacaoRepository;
        
        public TransacaoService(CartaoService _cartaoService,TransacaoRepository _transacaoRepository)
        {
            cartaoService = _cartaoService;
            transacaoRepository = _transacaoRepository;
        }
        public List<object> calculaTransacao (TransacaoDTO transacaoDTO)
        {
            decimal montanteTotal = transacaoDTO.ValorTotal * (1 + transacaoDTO.TaxaDeJuros);

            decimal valorParcela = montanteTotal / transacaoDTO.QtdeParcelas;

            var parcelas = new List<object>();
            for (int i = 1; i <= transacaoDTO.QtdeParcelas; i++)
            {
                parcelas.Add(new { parcela = i, valor = Math.Round(valorParcela, 2) });
            }
            return parcelas;
        }


        public bool RealizarPagamento(CadastrarTransacaoDTO transacaoDTO)
        {
            bool sucesso;

            Transacao transacao = new Transacao();

            transacao.Valor = transacaoDTO.Valor;
            transacao.CVV = transacaoDTO.CVV;
            transacao.Cartao = transacaoDTO.Cartao;
            transacao.Parcelas = transacaoDTO.Parcelas;
            transacao.Situacao = 1;

            sucesso = cartaoService.ObterCartaoValido(transacao.Cartao);
            if (!sucesso)
                return false;
            else
            {
                sucesso = transacaoRepository.Cadastrar(transacao);
                return sucesso;
            }
        }

        public int VerificaSituacao(int transacaoId)
        {

            var valor = transacaoRepository.VerificaSituacao(transacaoId);

            return valor;

        }

        public bool ConfirmaTransacao(int transacaoId)
        {
            var valor = transacaoRepository.VerificaSituacao(transacaoId);
            bool sucesso;
            if(valor == 3)
            {
                sucesso = false;
            }
            else
            {
                sucesso = transacaoRepository.Alterar(transacaoId);
            } 
            return sucesso;
        }

        public bool CancelaTransacao(int transacaoId)
        {
            var valor = transacaoRepository.VerificaSituacao(transacaoId);
            bool sucesso;
            if (valor == 2)
            {
                sucesso = false;
            }
            else
            {
                sucesso = transacaoRepository.Alterar2(transacaoId);
            }
            return sucesso;
        }

    }
}
