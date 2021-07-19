using Microsoft.AspNetCore.Mvc;
using NSE.Clientes.API.Application.Commands;
using NSE.Core.Mediator;
using NSE.WebApi.Core.Controllers;
using System;
using System.Threading.Tasks;
using NSE.Clientes.API.Models;
using NSE.WebApi.Core.Usuario;

namespace NSE.Clientes.API.Controllers
{
    [Route("cliente")]
    public class ClientesController : MainController
    {
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IClienteRepository _clienteRepository;
        private readonly IAspNetUser _user;

        public ClientesController(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        [HttpGet("endereco")]
        public async Task<IActionResult> ObterEndereco()
        {

            var endereco = await _clienteRepository.ObterEnderecoPorClienteIdAsync(_user.ObterUserId());
            return endereco == null ? NotFound() : CustomResponse(endereco);
        }

        
        [HttpPost("endereco")]
        public async Task<IActionResult> AdicionarEndereco(AdicionarEnderecoCommand endereco)
        {

            endereco.ClienteId = _user.ObterUserId();
            return CustomResponse(await _mediatorHandler.EnviarComando(endereco));
        }
    }
}
