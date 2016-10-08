using System.Data.Entity.ModelConfiguration;
using Domain.Misc;

namespace Persistence.Mappers
{ 
    public class MeasureUnitMap : EntityTypeConfiguration<MeasureUnit>
    {
		public MeasureUnitMap()
        {
            this.ToTable("MeasureUnits");
            this.HasKey(measureunit => measureunit.Id);

            this.Property(measureunit => measureunit.Id).IsRequired();
        }
	}
}