using NSE.WebApp.MVC.Models;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services
{
    public interface IAutenticacaoService
    {
        Task<UsuarioRespostaLogin> Register(UsuarioRegistro usuario);
        Task<UsuarioRespostaLogin> Login(UsuarioLogin login);
        Task RealizarLogin(UsuarioRespostaLogin resposta);
        Task<UsuarioRespostaLogin> UtilizarRefreshToken(string refreshToken);
        Task Logout();
        bool TokenExpirado();
        Task<bool> RefreshTokenEhValido();
    }
}
