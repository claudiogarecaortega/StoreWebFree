using System.Data.Entity.ModelConfiguration;
using Domain.Providers;

namespace Persistence.Mappers
{ 
    public class ProviderMap : EntityTypeConfiguration<Provider>
    {
		public ProviderMap()
        {
            this.ToTable("Providers");
            this.HasKey(provider => provider.Id);

            this.Property(provider => provider.Id).IsRequired();
        }
	}
}