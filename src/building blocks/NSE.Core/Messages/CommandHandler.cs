using FluentValidation.Results;
using NSE.Core.DomainObjects.Data;
using System.Threading.Tasks;

namespace NSE.Core.Messages
{
    public abstract class CommandHandler
    {
        protected ValidationResult ValidationResult;

        protected CommandHandler()
        {
            ValidationResult = new ValidationResult();
        }

        protected void AdicionarErro(string mensagem)
        {
            ValidationResult.Errors.Add(new ValidationFailure(string.Empty, mensagem));
        }

        protected async Task<ValidationResult> PersistirDados(IUnitOfWork unitOfWork)
        {
            var sucesso = await unitOfWork.Commit();
            if (!sucesso)
            {
                AdicionarErro("Houve um erro ao persistir os dados.");
            }
            return ValidationResult;
        }
    }
}
