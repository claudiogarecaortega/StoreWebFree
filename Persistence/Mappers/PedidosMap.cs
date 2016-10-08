using System.Data.Entity.ModelConfiguration;
using Domain.Ventas;

namespace Persistence.Mappers
{ 
    public class PedidosMap : EntityTypeConfiguration<Pedidos>
    {
		public PedidosMap()
        {
            this.ToTable("Pedidoss");
            this.HasKey(pedidos => pedidos.Id);

            this.Property(pedidos => pedidos.Id).IsRequired();
        }
	}
}