using System.Data.Entity.ModelConfiguration;
using Domain.Contable;

namespace Persistence.Mappers
{ 
    public class CuotasMap : EntityTypeConfiguration<Cuotas>
    {
		public CuotasMap()
        {
            this.ToTable("Cuotass");
            this.HasKey(cuotas => cuotas.Id);

            this.Property(cuotas => cuotas.Id).IsRequired();
        }
	}
}