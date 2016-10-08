using System.Data.Entity.ModelConfiguration;
using Domain.Misc;

namespace Persistence.Mappers
{ 
    public class UbicationMap : EntityTypeConfiguration<Ubication>
    {
		public UbicationMap()
        {
            this.ToTable("Ubications");
            this.HasKey(ubication => ubication.Id);

            this.Property(ubication => ubication.Id).IsRequired();
        }
	}
}