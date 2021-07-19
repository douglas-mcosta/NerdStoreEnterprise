using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NSE.Pedidos.Domain.Pedidos;

namespace NSE.Pedidos.Infra.Data.Mappings
{
    public class PedidoItemMapping : IEntityTypeConfiguration<PedidoItem>
    {
        public void Configure(EntityTypeBuilder<PedidoItem> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.ProdutoNome)
                .IsRequired()
                .HasColumnType("varchar(250)");

            builder.HasOne(i => i.Pedido)
                .WithMany(p => p.PedidoItems);

            builder.ToTable("PedidoItems");
        }
    }
}