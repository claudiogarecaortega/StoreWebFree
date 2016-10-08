using System.Data.Entity.ModelConfiguration;
using Domain.Misc;

namespace Persistence.Mappers
{ 
    public class BillTypeMap : EntityTypeConfiguration<BillType>
    {
		public BillTypeMap()
        {
            this.ToTable("BillTypes");
            this.HasKey(billtype => billtype.Id);

            this.Property(billtype => billtype.Id).IsRequired();
        }
	}
}