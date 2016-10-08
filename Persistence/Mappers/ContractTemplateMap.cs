using System.Data.Entity.ModelConfiguration;
using Domain.Misc;

namespace Persistence.Mappers
{ 
    public class ContractTemplateMap : EntityTypeConfiguration<ContractTemplate>
    {
		public ContractTemplateMap()
        {
            this.ToTable("ContractTemplates");
            this.HasKey(contracttemplate => contracttemplate.Id);

            this.Property(contracttemplate => contracttemplate.Id).IsRequired();
        }
	}
}