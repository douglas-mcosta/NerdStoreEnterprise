using DevIO.Api.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services
{
    public interface IAutenticacaoService
    {
        Task<UsuarioRespostaLogin> Register(UsuarioRegistro usuario);
        Task<UsuarioRespostaLogin> Login(UsuarioLogin login);
    }
}
