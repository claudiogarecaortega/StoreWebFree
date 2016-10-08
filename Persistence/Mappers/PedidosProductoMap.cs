using System.Data.Entity.ModelConfiguration;
using Domain.Providers;

namespace Persistence.Mappers
{ 
    public class PedidosProductoMap : EntityTypeConfiguration<PedidosProducto>
    {
		public PedidosProductoMap()
        {
            this.ToTable("PedidosProductos");
            this.HasKey(pedidosproducto => pedidosproducto.Id);

            this.Property(pedidosproducto => pedidosproducto.Id).IsRequired();
		    this.HasRequired(m => m.CuentasPagar)
		        .WithOptional(u => u.Credito)
		        .Map(r => r.MapKey("PedidoProductoId"));
		    this.HasMany(m => m.Productos)
		        .WithMany(f => f.Pedidos)
		        .Map(o =>
		        {
		            o.ToTable("PedidosProductosMany");
		            o.MapLeftKey("PedidoId");
		            o.MapRightKey("ProductoId");
		        });

        }
	}
}