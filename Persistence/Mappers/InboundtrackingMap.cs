using System.Data.Entity.ModelConfiguration;
using Domain.Commodity;

namespace Persistence.Mappers
{ 
    public class InboundtrackingMap : EntityTypeConfiguration<InboundTracking>
    {
		public InboundtrackingMap()
        {
            this.ToTable("Inboundtrackings");
            this.HasKey(inboundtracking => inboundtracking.Id);

            this.Property(inboundtracking => inboundtracking.Id).IsRequired();
        }
	}
}