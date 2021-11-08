using Microsoft.Extensions.Hosting;
using NSE.Core.Messages.Integration;
using NSE.MessageBus;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System;
using NSE.Pagamentos.API.Models;
using NSE.Core.DomainObjects;
using Microsoft.Extensions.Logging;

namespace NSE.Pagamentos.API.Services
{
    public class PagamentoIntegrationHandler : BackgroundService
    {
        private readonly ILogger<PagamentoIntegrationHandler> _log;
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public PagamentoIntegrationHandler(IMessageBus bus, IServiceProvider serviceProvider, ILogger<PagamentoIntegrationHandler> log)
        {
            _bus = bus;
            _serviceProvider = serviceProvider;
            _log = log;
        }


        private void SetResponder()
        {
            _bus.RespondAsync<PedidoIniciadoIntegrationEvent, ResponseMessage>(async request =>
            await AutorizarPagamento(request));
        }

        private void SetSubscribe()
        {
            _bus.SubscribeAsync<PedidoCanceladoIntegrationEvent>("PedidoCancelado", async request =>
            {
                _log.LogInformation("Iniciando Cancelamento de Pedido");
                await CancelarPagamento(request);
            }
            );

            _bus.SubscribeAsync<PedidoBaixadoEstoqueIntegrationEvent>("PedidoBaixaEstoque", async request => 
            await CapturarPagamento(request));
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetResponder();
            SetSubscribe();
            return Task.CompletedTask;
        }

        private async Task<ResponseMessage> AutorizarPagamento(PedidoIniciadoIntegrationEvent message)
        {
            _log.LogInformation("Iniciando AutorizarPagamento de Pedido");

            ResponseMessage response;

            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                var pagamentoService = scope.ServiceProvider.GetRequiredService<IPagamentoService>();

                var pagamento = new Pagamento
                {
                    PedidoId = message.PedidoId,
                    TipoPagamento = (TipoPagamento)message.TipoPagamento,
                    Valor = message.Valor,
                    CartaoCredito = new CartaoCredito(message.NomeCartao, message.NumeroCartao, message.MesAnoVencimento, message.CVV)
                };

                response = await pagamentoService.AutorizarPagamento(pagamento);

            }
            return response;
        }
         private async Task CancelarPagamento(PedidoCanceladoIntegrationEvent message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var pagamentoService = scope.ServiceProvider.GetRequiredService<IPagamentoService>();

                var response = await pagamentoService.CancelarPagamento(message.PedidoId);

                if (!response.ValidationResult.IsValid)
                    throw new DomainException($"Falha ao cancelar pagamento do pedido {message.PedidoId}");
            }
        }

        private async Task CapturarPagamento(PedidoBaixadoEstoqueIntegrationEvent message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var pagamentoService = scope.ServiceProvider.GetRequiredService<IPagamentoService>();

                var response = await pagamentoService.CapturarPagamento(message.PedidoId);

                if (!response.ValidationResult.IsValid)
                    throw new DomainException($"Falha ao capturar pagamento do pedido {message.PedidoId}");

                await _bus.PublishAsync(new PedidoPagoIntegrationEvent(message.ClienteId, message.PedidoId));
            }
        }
    }
}
