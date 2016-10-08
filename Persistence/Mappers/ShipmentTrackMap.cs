using System.Data.Entity.ModelConfiguration;
using Domain.Products;

namespace Persistence.Mappers
{ 
    public class ShipmentTrackMap : EntityTypeConfiguration<ShipmentTrack>
    {
		public ShipmentTrackMap()
        {
            this.ToTable("ShipmentTracks");
            this.HasKey(shipmenttrack => shipmenttrack.Id);

            this.Property(shipmenttrack => shipmenttrack.Id).IsRequired();
        }
	}
}