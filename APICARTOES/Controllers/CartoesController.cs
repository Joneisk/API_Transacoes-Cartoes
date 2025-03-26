using APICARTOES.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APICARTOES.Controllers
{
    /// <summary>
    /// Controle de Cartões
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CartoesController : ControllerBase
    {
        public readonly CartaoService cartaoService;

        
        public CartoesController(CartaoService _cartaoservice)
        {
            cartaoService = _cartaoservice;
        }
        
       
        /// <summary>
        /// Obtem a bandeira do cartão
        /// </summary>
        /// <param name="cartao"></param>
        /// <returns></returns>
        [HttpGet("{cartao}/obter-bandeira")]
        public ActionResult GetCartoes(string cartao)
        {
            if (string.IsNullOrWhiteSpace(cartao))
                return NotFound("Cartão Vazio");

            var tipoCartao = cartaoService.ObterPorNumero(cartao);

            if (string.IsNullOrWhiteSpace(tipoCartao))
                return NotFound("Cartão não encontrado");

            return Ok(tipoCartao);
        }

        /// <summary>
        /// Verifica se o Cartão é valido
        /// </summary>
        /// <param name="cartao"></param>
        /// <returns></returns>
        [HttpGet("{cartao}/valido")]
        public ActionResult GetCartaoValido(string cartao)
        {

            if (cartao == null)
                return NotFound();

            var sucesso = cartaoService.ObterCartaoValido(cartao);

            return Ok(sucesso);

        }

    }
}
