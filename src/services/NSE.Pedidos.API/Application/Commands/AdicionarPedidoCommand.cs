using System;
using System.Collections.Generic;
using FluentValidation;
using NSE.Core.Messages;
using NSE.Pedidos.API.Application.DTO;

namespace NSE.Pedidos.API.Application.Commands
{
    public class AdicionarPedidoCommand : Command
    {
        //Pedido
        public Guid ClienteId { get; private set; }
        public decimal ValorTotal { get; private set; }
        public List<PedidoItemDTO> PedidoItems { get; private set; }

        //Voucher
        public string VoucherCodigo { get; private set; }
        public bool VoucherUtilizado { get; private set; }
        public decimal Desconto { get; private set; }

        //Endereco
        public EnderecoDTO Endereco { get; private set; }

        //Cartao
        public string NumeroCartao { get; private set; }
        public string NomeCartao { get; private set; }
        public string ExpiracaoCartao { get; private set; }
        public string CvvCartao { get; private set; }

        public override bool EhValido()
        {
            ValidationResult = new AdicionarPedidoValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class AdicionarPedidoValidation : AbstractValidator<AdicionarPedidoCommand>
        {
            public AdicionarPedidoValidation()
            {
                RuleFor(c => c.ClienteId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Id do cliente inválido");

                RuleFor(c => c.PedidoItems.Count)
                    .GreaterThan(0)
                    .WithMessage("O pedido precisa ter no mínimo 1 item");

                RuleFor(c => c.ValorTotal)
                    .GreaterThan(0)
                    .WithMessage("Valor do pedido inválido");

                RuleFor(c => c.NumeroCartao)
                    .CreditCard()
                    .WithMessage("Número de cartão inválido");

                RuleFor(c => c.NomeCartao)
                    .NotNull()
                    .WithMessage("Nome do portador do cartão requerido.");

                RuleFor(c => c.CvvCartao.Length)
                    .GreaterThan(2)
                    .LessThan(5)
                    .WithMessage("O CVV do cartão precisa ter 3 ou 4 números.");

                RuleFor(c => c.ExpiracaoCartao)
                    .NotNull()
                    .WithMessage("Data expiração do cartão requerida.");
            }
        }

    }
}