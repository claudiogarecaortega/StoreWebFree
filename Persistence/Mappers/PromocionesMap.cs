using System.Data.Entity.ModelConfiguration;
using Domain.Products;

namespace Persistence.Mappers
{ 
    public class PromocionesMap : EntityTypeConfiguration<Promociones>
    {
		public PromocionesMap()
        {
            this.ToTable("Promocioness");
            this.HasKey(promociones => promociones.Id);

            this.Property(promociones => promociones.Id).IsRequired();
        }
	}
}