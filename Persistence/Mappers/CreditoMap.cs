using System.Data.Entity.ModelConfiguration;
using Domain.Contable;

namespace Persistence.Mappers
{ 
    public class CreditoMap : EntityTypeConfiguration<Credito>
    {
		public CreditoMap()
        {
            this.ToTable("Creditos");
            this.HasKey(credito => credito.Id);

            this.Property(credito => credito.Id).IsRequired();
		    this.HasRequired(f => f.CuentasCobrar)
		        .WithOptional(f => f.Credito)
		        .Map(r=>r.MapKey("CreditoId"));
		    this.HasMany(f => f.Producto)
		        .WithMany(g => g.Creditos)
		        .Map(m =>
		        {
		            m.ToTable("CreditoProductos");
		            m.MapLeftKey("CreditoId");
		            m.MapRightKey("ProductoId");
		        });
        }
	}
}