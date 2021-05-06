using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NSE.Carrinho.API.Model
{
    public class CarrinhoCliente
    {
        internal const int MAX_QUANTIDADE_ITEM = 5;

        public CarrinhoCliente()
        {
            Itens = new List<CarrinhoItem>();
        }

        public CarrinhoCliente(Guid clienteId) : this()
        {
            Id = Guid.NewGuid();
            ClienteId = clienteId;
        }

        public Guid Id { get; private set; }
        public Guid ClienteId { get; private set; }
        public decimal ValorTotal { get; private set; }
        public List<CarrinhoItem> Itens { get; private set; }
        public ValidationResult ValidationResult { get; private set; }

        internal CarrinhoItem ObterItemPorProdutoId(Guid produtoId)
        {
            return Itens.FirstOrDefault(i => i.ProdutoId == produtoId);
        }

        internal void CalcularValorCarrinho()
        {
            ValorTotal = Itens.Sum(p => p.CalcularValor());
        }

        internal bool CarrinhoItemExistente(CarrinhoItem item)
        {
            return Itens.Any(i => i.ProdutoId == item.ProdutoId);
        }

        internal void AdicionarItem(CarrinhoItem item)
        {
            item.AssociarCarrinhoItem(Id);
            if (CarrinhoItemExistente(item))
            {
                var itemExistente = ObterItemPorProdutoId(item.ProdutoId);
                itemExistente.AdicionarUnidades(item.Quantidade);

                item = itemExistente;
                Itens.Remove(item);
            }
            Itens.Add(item);
            CalcularValorCarrinho();
        }

        internal void AtualizarItem(CarrinhoItem item)
        {
            item.AssociarCarrinhoItem(Id);

            var itemExistente = ObterItemPorProdutoId(item.ProdutoId);

            Itens.Remove(itemExistente);
            Itens.Add(item);

            CalcularValorCarrinho();
        }

        internal void AtualizarUnidades(CarrinhoItem item, int unidades)
        {
            item.AtualizarUnidades(unidades);
            AtualizarItem(item);
        }

        internal void RemoverItem(CarrinhoItem item)
        {
            var carrinhoItem = ObterItemPorProdutoId(item.ProdutoId);
            Itens.Remove(carrinhoItem);
            CalcularValorCarrinho();
        }

        internal bool EhValido()
        {
            var erros = Itens.SelectMany(i => new CarrinhoItem.ItemCarrinhoValidation()
                .Validate(i).Errors)
                .ToList();

            ValidationResult = new ValidationResult(erros);
            erros.AddRange(new CarrinhoClienteValidation().Validate(this).Errors);

            return ValidationResult.IsValid;
        }

        public class CarrinhoClienteValidation : AbstractValidator<CarrinhoCliente>
        {
            public CarrinhoClienteValidation()
            {
                RuleFor(c => c.ClienteId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Cliente não reconhecido.");

                RuleFor(c => c.Itens.Count)
                    .GreaterThan(0)
                    .WithMessage("O carrinho não possui itens.");

                RuleFor(c => c.ValorTotal)
                    .GreaterThan(0)
                    .WithMessage("O valor total do carrinho precisa ser maior que 0.");
            }
        }
    }
}