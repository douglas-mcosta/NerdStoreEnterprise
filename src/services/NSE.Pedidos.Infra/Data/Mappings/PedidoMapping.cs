using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NSE.Pedidos.Domain.Pedidos;

namespace NSE.Pedidos.Infra.Data.Mappings
{
    public class PedidoMapping : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {

            builder.HasKey(p => p.Id);

            builder.OwnsOne(p => p.Endereco, e =>
             {
                 e.Property(pe => pe.Logradouro)
                     .HasColumnName("Logradouro");

                 e.Property(pe => pe.Numero)
                     .HasColumnName("Numero");

                 e.Property(pe => pe.Complemento)
                     .HasColumnName("Complemento");
                 
                 e.Property(pe => pe.Bairro)
                     .HasColumnName("Bairro");

                 e.Property(pe => pe.Cep)
                     .HasColumnName("Cep");

                 e.Property(pe => pe.Cidade)
                     .HasColumnName("Cidade");

                 e.Property(pe => pe.Estado)
                     .HasColumnName("Estado");
             });

            builder.Property(p => p.Codigo)
                .HasDefaultValueSql("NEXT VALUE FOR SequenciaCodigoPedido");

            builder.HasMany(p => p.PedidoItems)
                .WithOne(i => i.Pedido)
                .HasForeignKey(i => i.PedidoId);

            builder.ToTable("Pedidos");
        }
    }
}