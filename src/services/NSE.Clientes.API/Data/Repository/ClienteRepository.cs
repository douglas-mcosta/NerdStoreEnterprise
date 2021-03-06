using Microsoft.EntityFrameworkCore;
using NSE.Clientes.API.Models;
using NSE.Core.DomainObjects.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NSE.Clientes.API.Data.Repository
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ClienteContext _clienteContext;

        public ClienteRepository(ClienteContext clienteContext)
        {
            _clienteContext = clienteContext;
        }

        public IUnitOfWork UnitOfWork => _clienteContext;

        public async Task<IEnumerable<Cliente>> ObterTodos()
        {
            return await _clienteContext.Clientes.AsNoTracking().ToListAsync();
        }

        public  Task<Cliente> ObterPorCpf(string cpf)
        {
            return _clienteContext.Clientes.FirstOrDefaultAsync(cliente => cliente.Cpf.Numero == cpf);
        }

        public void Adicionar(Cliente cliente)
        {
            _clienteContext.Clientes.Add(cliente);
        }

        public async Task<Endereco> ObterEnderecoPorClienteIdAsync(Guid clienteId)
        {
            return await _clienteContext.Endereco.FirstOrDefaultAsync(x=>x.ClienteId == clienteId);
        }

         public async Task AdicionarEnderecoAsync(Endereco endereco)
        {
           await _clienteContext.Endereco.AddAsync(endereco);
        }


        public void Dispose()
        {
            _clienteContext.Dispose();
        }

       
    }
}
