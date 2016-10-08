using System.Data.Entity.ModelConfiguration;
using Domain.Contable;

namespace Persistence.Mappers
{ 
    public class CuentasPagarMap : EntityTypeConfiguration<CuentasPagar>
    {
		public CuentasPagarMap()
        {
            this.ToTable("CuentasPagars");
            this.HasKey(cuentaspagar => cuentaspagar.Id);

            this.Property(cuentaspagar => cuentaspagar.Id).IsRequired();
        }
	}
}