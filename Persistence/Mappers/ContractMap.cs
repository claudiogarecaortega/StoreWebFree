using System.Data.Entity.ModelConfiguration;
using Domain.Misc;

namespace Persistence.Mappers
{ 
    public class ContractMap : EntityTypeConfiguration<Contract>
    {
		public ContractMap()
        {
            this.ToTable("Contracts");
            this.HasKey(contract => contract.Id);

            this.Property(contract => contract.Id).IsRequired();
        }
	}
}