using Microsoft.EntityFrameworkCore;
using NSE.Catalogo.API.Data;
using NSE.Core.DomainObjects.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NSE.Catalogo.API.Models
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly CatalogoContext _context;

        public ProdutoRepository(CatalogoContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;
        public async Task<Produto> ObterPorId(Guid id)
        {
            return await _context.produtos.FindAsync(id);
        }

        public async Task<IEnumerable<Produto>> ObterTodos()
        {
            return await _context.produtos.AsNoTracking().ToListAsync();
        }

        public void Adicionar(Produto produto)
        {
            _context.produtos.Add(produto);
        }

        public void Atualizar(Produto produto)
        {
            _context.produtos.Update(produto);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
