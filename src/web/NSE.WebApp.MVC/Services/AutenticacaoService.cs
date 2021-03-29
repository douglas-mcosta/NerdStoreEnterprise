using DevIO.Api.ViewModels;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services
{
    public class AutenticacaoService : IAutenticacaoService
    {
        private readonly HttpClient _httpClient;

        public AutenticacaoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UsuarioRespostaLogin> Login(UsuarioLogin login)
        {
            var loginContent = new StringContent(
                JsonSerializer.Serialize(login),
                Encoding.UTF8,
                "application/json"
                );
           var response = await _httpClient.PostAsync("https://localhost:44355/api/identidade/autenticar", loginContent);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<UsuarioRespostaLogin>(await response.Content.ReadAsStringAsync(),options);
        }

        public async Task<UsuarioRespostaLogin> Register(UsuarioRegistro usuario)
        {
            var usuarioContent = new StringContent(
               JsonSerializer.Serialize(usuario),
               Encoding.UTF8,
               "application/json"
               );
            var response = await _httpClient.PostAsync("https://localhost:44355/api/identidade/nova-conta", usuarioContent);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<UsuarioRespostaLogin>(await response.Content.ReadAsStringAsync(),options);
        }
    }
}
