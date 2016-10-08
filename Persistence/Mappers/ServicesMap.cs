using System.Data.Entity.ModelConfiguration;
using System.Runtime.CompilerServices;
using Domain.Misc;

namespace Persistence.Mappers
{ 
    public class ServicesMap : EntityTypeConfiguration<Services>
    {
		public ServicesMap()
        {
            this.ToTable("Servicess");
            this.HasKey(services => services.Id);

            this.Property(services => services.Id).IsRequired();
            //HasMany(d=>d.Serviceses).WithMany(d=>d.Serviceses)
            //   .Map(cs =>
            //   {
            //       cs.MapLeftKey("ClientId");
            //       cs.MapRightKey("ServiceId");
            //       cs.ToTable("ClienService");
            //   });
            
        }
	}
}