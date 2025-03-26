using APICARTOES.DTOs;
using APICARTOES.Models;
using APICARTOES.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APICARTOES.Controllers
{
    [Route("api/pagamentos")]
    [ApiController]
    public class TransacoesController : ControllerBase
    {
        private readonly TransacaoService tService;


        public TransacoesController(TransacaoService _tService)
        {
            tService = _tService;
        }


        /// <summary>
        /// Calcula as parcelas de uma transacao
        /// </summary>
        /// <param name="transacaoDTO"></param>
        /// <returns></returns>
        [HttpPost("calcular-parcelas")]
        public ActionResult<IEnumerable<object>> calculaParcelas([FromBody] TransacaoDTO transacaoDTO)
        {
            if (transacaoDTO == null)
            {
                return BadRequest("Enviar JSON Correto");
            }
            if (transacaoDTO.QtdeParcelas <= 0) // Melhor ajuste na validação
            {
                return BadRequest("Quantidade de parcelas deve ser maior que 0");
            }

            var list = tService.calculaTransacao(transacaoDTO);
            return Ok(list);
        }

        /// <summary>
        /// Cria uma transacao
        /// </summary>
        /// <param name="transacao"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Pagamentos(CadastrarTransacaoDTO transacao)
        {
            if (transacao == null)
                return BadRequest();
            bool sucesso = tService.RealizarPagamento(transacao);

            if (!sucesso)
                return BadRequest("Erro ao cadastrar Transacao");
            else
                return Ok("Transacao Cadastrada com sucesso.");
        }


        /// <summary>
        /// Verifica a situaçao da Transação
        /// </summary>
        /// <param name="transacaoid"></param>
        /// <returns></returns>
        [HttpGet("{transacaoid}/situacao")]
        public ActionResult Situacao(int transacaoid)
        {
            if (transacaoid < 0)
                return BadRequest();
            

            var situacao = tService.VerificaSituacao(transacaoid);
            if (situacao == 0)
                return BadRequest();
            else
                return Ok(situacao);

        }

        /// <summary>
        /// Confirma a transacão
        /// </summary>
        /// <param name="transacaoid"></param>
        /// <returns></returns>
        [HttpPut("{transacaoid}/confirmar")]
        public ActionResult Confirmar(int transacaoid)
        {
            if (transacaoid < 0)
                return BadRequest();


            bool sucesso = tService.ConfirmaTransacao(transacaoid);
            if (sucesso == false)
                return BadRequest("Erro ao Confirmar Transacao");
            else
                return Ok(sucesso);

        }
        /// <summary>
        /// Cancela a transação
        /// </summary>
        /// <param name="transacaoid"></param>
        /// <returns></returns>
        [HttpPut("{transacaoid}/cancelar")]
        public ActionResult Cancelar(int transacaoid)
        {
            if (transacaoid < 0)
                return BadRequest();


            bool sucesso = tService.CancelaTransacao(transacaoid);
            if (sucesso == false)
                return BadRequest("Erro ao Confirmar Transacao");
            else
                return Ok(sucesso);

        }

    }
}
