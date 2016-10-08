using System.Data.Entity.ModelConfiguration;
using Domain.Contable;

namespace Persistence.Mappers
{ 
    public class CuentasCobrarMap : EntityTypeConfiguration<CuentasCobrar>
    {
		public CuentasCobrarMap()
        {
            this.ToTable("CuentasCobrars");
            this.HasKey(cuentascobrar => cuentascobrar.Id);

            this.Property(cuentascobrar => cuentascobrar.Id).IsRequired();
        }
	}
}