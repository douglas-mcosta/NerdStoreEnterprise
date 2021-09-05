using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetDevPack.Security.Jwt.Interfaces;
using NSE.Core.Messages.Integration;
using NSE.Identidade.API.Models;
using NSE.MessageBus;
using NSE.WebApi.Core.Controllers;
using NSE.WebApi.Core.Identidate;
using NSE.WebApi.Core.Usuario;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NSE.Identidade.API.Controllers
{
    [Route("api/identidade")]
    public class AuthController : MainController
    {

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly Token _appSettings;
        private readonly IMessageBus _bus;
        private readonly IAspNetUser _aspNetUser;
        private readonly IJsonWebKeySetService _jsonWebKeySetService;

        public AuthController(SignInManager<IdentityUser> signInManager,
                              UserManager<IdentityUser> userManager,
                              IOptions<Token> appSettings, IMessageBus bus,
                              IAspNetUser aspNetUser,
                              IJsonWebKeySetService jsonWebKeySetService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
            _bus = bus;
            _aspNetUser = aspNetUser;
            _jsonWebKeySetService = jsonWebKeySetService;
        }

        [HttpPost("nova-conta")]
        public async Task<IActionResult> Registrar(UsuarioRegistro usuario)
        {

            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            var user = new IdentityUser
            {
                Email = usuario.Email,
                UserName = usuario.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, usuario.Senha);

            if (result.Succeeded)
            {
                var clienteResult = await RegistrarCliete(usuario);

                if (!clienteResult.ValidationResult.IsValid)
                {
                    await _userManager.DeleteAsync(user);
                    return CustomResponse(clienteResult.ValidationResult);
                }

                return CustomResponse(await GerarJwt(usuario.Email));
            }

            foreach (var erro in result.Errors)
            {
                AdicionarErroProcessamento(erro.Description);
            }

            return CustomResponse();
        }


        [HttpPost("autenticar")]
        public async Task<IActionResult> Login(UsuarioLogin usuario)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            var result = await _signInManager.PasswordSignInAsync(usuario.Email, usuario.Senha, false, true);

            if (result.Succeeded)
            {
                return CustomResponse(await GerarJwt(usuario.Email));
            }

            if (result.IsLockedOut)
            {
                AdicionarErroProcessamento("Usuário temporariamente bloqueado por tentativas invalidas.");
                return CustomResponse();
            }

            AdicionarErroProcessamento("Usuário ou Senha invalido.");
            return CustomResponse();
        }

        private async Task<UsuarioRespostaLogin> GerarJwt(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var claims = await _userManager.GetClaimsAsync(user);

            var identityClaims = await ObterClaimsUauario(claims, user);
            var encodedToken = CodificarToken(identityClaims);

            return ObterRespostaToken(encodedToken, user, claims);
        }

        private async Task<ClaimsIdentity> ObterClaimsUauario(ICollection<Claim> claims, IdentityUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));
            claims.Add(new Claim(JwtRegisteredClaimNames.Aud, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim("role", userRole));
            }

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            return identityClaims;
        }

        private string CodificarToken(ClaimsIdentity claims)
        {
            var currentIssuer = $"{_aspNetUser.ObterHttpContext().Request.Scheme}://{_aspNetUser.ObterHttpContext().Request.Host}";
            //Para manipular o token
            var tokenHandle = new JwtSecurityTokenHandler();
            //Key
            var key = _jsonWebKeySetService.GetCurrentSigningCredentials();
            //Gerar o token
            var token = tokenHandle.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = currentIssuer,
                Subject = claims,
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = key
            });
            //Escrever o token. Serializar no padrão da web
            var encodedToken = tokenHandle.WriteToken(token);
            return encodedToken;
        }

        private UsuarioRespostaLogin ObterRespostaToken(string encodedToken, IdentityUser user, IEnumerable<Claim> claims)
        {

            var filtro = new List<string>(){
                new string("sub"),
                new string("jti"),
                new string("nbf"),
                new string("iat"),
                new string("iss"),
                new string("aud"),
                new string("email"),
            };
            return new UsuarioRespostaLogin
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpiracaoHoras).TotalSeconds,
                UsuarioToken = new UsuarioToken
                {
                    Email = user.Email,
                    Id = user.Id,
                    Claims = claims.Select(x => new UsuarioClaim { Type = x.Type, Value = x.Value }).Where(x => !filtro.Contains(x.Type))
                }
            };
        }

        private static long ToUnixEpochDate(DateTime date)
       => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        private async Task<ResponseMessage> RegistrarCliete(UsuarioRegistro usuarioRegistro)
        {
            var usuario = await _userManager.FindByEmailAsync(usuarioRegistro.Email);
            var usuarioRegistrado = new UsuarioRegistradoIntegrationEvent(Guid.Parse(usuario.Id), usuarioRegistro.Nome, usuarioRegistro.Email, usuarioRegistro.Cpf);

            try
            {
                return await _bus.RequestAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(usuarioRegistrado);
            }
            catch 
            {
                await _userManager.DeleteAsync(usuario);
                throw;
            }

        }
    }
}
