using System.Data.Entity.ModelConfiguration;
using Domain.Almacen;

namespace Persistence.Mappers
{ 
    public class AlmacenMap : EntityTypeConfiguration<Almacen>
    {
		public AlmacenMap()
        {
            this.ToTable("Almacens");
            this.HasKey(almacen => almacen.Id);

            this.Property(almacen => almacen.Id).IsRequired();
        }
	}
}