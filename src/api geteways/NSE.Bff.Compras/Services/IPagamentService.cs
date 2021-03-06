using Microsoft.Extensions.Options;
using NSE.Bff.Compras.Extensions;
using System;
using System.Net.Http;

namespace NSE.Bff.Compras.Services
{
    public interface IPagamentService
    {
    }

    public class PagamentoService : IPagamentService
    {
        private readonly HttpClient _httpClient;
        public PagamentoService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.CarrinhoUrl);
        }
    }
}