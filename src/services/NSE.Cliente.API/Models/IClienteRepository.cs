using System;
using NSE.Core.DomainObjects.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NSE.Clientes.API.Models
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        void Adicionar(Cliente cliente);

        Task<IEnumerable<Cliente>> ObterTodos();
        Task<Cliente> ObterPorCpf(string cpf);
        Task<Endereco> ObterEnderecoPorClienteIdAsync(Guid clienteId);
        Task AdicionarEnderecoAsync(Endereco endereco);

    }
}
