using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Identidade.API.Models
{
    public class UsuarioRegistro
    {
        [Required(ErrorMessage = " O campo {0} é obrigatorio.")]
        [EmailAddress(ErrorMessage ="O campo {0} está em um formato invalido.")]
        public string Email { get; set; }
        [Required(ErrorMessage = " O campo {0} é obrigatorio.")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres.",MinimumLength = 6)]
        public string Senha { get; set; }
        [Required(ErrorMessage = " O campo {0} é obrigatorio.")]
        [Compare("Senha",ErrorMessage ="As senhas não conferem.")]
        public string SenhaConfirmacao{ get; set; }
    }
    public class UsuarioLogin {
        [Required(ErrorMessage = " O campo {0} é obrigatorio.")]
        [EmailAddress(ErrorMessage = "O campo {0} está em um formato invalido.")]
        public string Email { get; set; }
        [Required(ErrorMessage = " O campo {0} é obrigatorio.")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres.", MinimumLength = 6)]
        public string Senha { get; set; }
    }

    public class UsuarioRespostaLogin
    {
        public string AccessToken { get; set; }
        public double ExpiratioIn { get; set; }
        public UsuarioToken UserToken { get; set; }
    }
    public class UsuarioToken
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public IEnumerable<USuarioClaim> Claims { get; set; }
    }
    public class USuarioClaim
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }

}
