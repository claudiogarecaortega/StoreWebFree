using System.Data.Entity.ModelConfiguration;
using System.Security.Policy;
using Domain.Products;

namespace Persistence.Mappers
{ 
    public class ShipmentMap : EntityTypeConfiguration<Shipment>
    {
		public ShipmentMap()
        {
            this.ToTable("Shipments");
            this.HasKey(shipment => shipment.Id);

            this.Property(shipment => shipment.Id).IsRequired();
		    this.HasMany(x => x.Cargars)
		        .WithOptional(x=>x.Envio)
		        .Map(x => x.MapKey("ShipmentId"));
		    this.HasMany(x => x.UbicationRoute)
		        .WithMany(x => x.Shipments)
                .Map(cs =>
                {
                    cs.MapLeftKey("ShipmentId");
                    cs.MapRightKey("UbicationId");
                    cs.ToTable("ShipmentUbication");
                });
        }
	}
}