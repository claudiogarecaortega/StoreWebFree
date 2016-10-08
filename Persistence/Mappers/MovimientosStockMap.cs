using System.Data.Entity.ModelConfiguration;
using Domain.Almacen;

namespace Persistence.Mappers
{ 
    public class MovimientosStockMap : EntityTypeConfiguration<MovimientosStock>
    {
		public MovimientosStockMap()
        {
            this.ToTable("MovimientosStocks");
            this.HasKey(movimientosstock => movimientosstock.Id);

            this.Property(movimientosstock => movimientosstock.Id).IsRequired();
        }
	}
}