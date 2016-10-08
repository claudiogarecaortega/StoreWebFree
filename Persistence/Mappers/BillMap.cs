using System.Data.Entity.ModelConfiguration;
using Domain.Misc;

namespace Persistence.Mappers
{ 
    public class BillMap : EntityTypeConfiguration<Bill>
    {
		public BillMap()
        {
            this.ToTable("Bills");
            this.HasKey(bill => bill.Id);

            this.Property(bill => bill.Id).IsRequired();
        }
	}
}