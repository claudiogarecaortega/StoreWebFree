using System.Data.Entity.ModelConfiguration;
using Domain.Commodity;

namespace Persistence.Mappers
{ 
    public class InboundMap : EntityTypeConfiguration<Inbound>
    {
		public InboundMap()
        {
            this.ToTable("Inbounds");
            this.HasKey(inbound => inbound.Id);

            this.Property(inbound => inbound.Id).IsRequired();
        }
	}
}