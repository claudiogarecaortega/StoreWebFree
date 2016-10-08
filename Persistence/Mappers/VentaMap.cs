using System.Data.Entity.ModelConfiguration;
using Domain.Ventas;

namespace Persistence.Mappers
{ 
    public class VentaMap : EntityTypeConfiguration<Venta>
    {
		public VentaMap()
        {
            this.ToTable("Ventas");
            this.HasKey(venta => venta.Id);

            this.Property(venta => venta.Id).IsRequired();
		    this.HasMany(d => d.Producto)
		        .WithMany(t => t.Ventas)
		        .Map(d =>
		        {
		            d.ToTable("VentasProductos");
		            d.MapLeftKey("VentasId");
		            d.MapRightKey("ProductoId");
		        });
        }
	}
}