using System.Data.Entity.ModelConfiguration;
using Domain.Almacen;

namespace Persistence.Mappers
{ 
    public class StockMap : EntityTypeConfiguration<Stock>
    {
		public StockMap()
        {
            this.ToTable("Stocks");
            this.HasKey(stock => stock.Id);

            this.Property(stock => stock.Id).IsRequired();
        }
	}
}