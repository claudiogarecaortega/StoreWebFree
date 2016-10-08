using System.Data.Entity.ModelConfiguration;
using Domain.Misc;

namespace Persistence.Mappers
{ 
    public class PackageTypeMap : EntityTypeConfiguration<PackageType>
    {
		public PackageTypeMap()
        {
            this.ToTable("PackageTypes");
            this.HasKey(packagetype => packagetype.Id);

            this.Property(packagetype => packagetype.Id).IsRequired();
        }
	}
}