using System.Data.Entity.ModelConfiguration;
using Domain.Misc;

namespace Persistence.Mappers
{ 
    public class CarMap : EntityTypeConfiguration<Car>
    {
		public CarMap()
        {
            this.ToTable("Cars");
            this.HasKey(car => car.Id);

            this.Property(car => car.Id).IsRequired();
        }
	}
}