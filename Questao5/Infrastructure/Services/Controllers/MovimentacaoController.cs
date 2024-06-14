using Microsoft.AspNetCore.Mvc;
using Questao5.Infrastructure.Models;
using System.Transactions;
using static Questao5.Infrastructure.Services.ContaBancariaService;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovimentacaoController : ControllerBase
    {
        private readonly IContaBancariaService _contaBancariaService;

        public MovimentacaoController(IContaBancariaService contaBancariaService)
        {
            _contaBancariaService = contaBancariaService;
        }

        [HttpPost(Name = "PostMovimentacao")]
        public async Task<IActionResult> PostMovimentacao([FromBody] Movimentacao movimentacao)
        {
            var result = await _contaBancariaService.ProcessarMovimentacaoAsync(movimentacao);

            return result switch
            {
                "INVALID_ACCOUNT" => BadRequest(new { error = "INVALID_ACCOUNT", message = "Conta não existe." }),
                "INACTIVE_ACCOUNT" => BadRequest(new { error = "INACTIVE_ACCOUNT", message = "Conta está inativa." }),
                "INVALID_VALUE" => BadRequest(new { error = "INVALID_VALUE", message = "Valor da Movimentação precisa ser positivo." }),
                "INVALID_TYPE" => BadRequest(new { error = "INVALID_TYPE", message = "Tipo da Transação deve ser 'C' para crédito ou 'D' para Débito." }),
                _ => Ok(new { message = result })
            };
        }
    }
}

