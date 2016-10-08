using System.Data.Entity.ModelConfiguration;
using Domain.Misc;

namespace Persistence.Mappers
{ 
    public class TaxConditionMap : EntityTypeConfiguration<TaxCondition>
    {
		public TaxConditionMap()
        {
            this.ToTable("TaxConditions");
            this.HasKey(taxcondition => taxcondition.Id);

            this.Property(taxcondition => taxcondition.Id).IsRequired();
        }
	}
}