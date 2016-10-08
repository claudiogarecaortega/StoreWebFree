using System.Data.Entity.ModelConfiguration;
using Domain.Contable;

namespace Persistence.Mappers
{ 
    public class BancariaMap : EntityTypeConfiguration<Bancaria>
    {
		public BancariaMap()
        {
            this.ToTable("Bancarias");
            this.HasKey(bancaria => bancaria.Id);

            this.Property(bancaria => bancaria.Id).IsRequired();
        }
	}
}