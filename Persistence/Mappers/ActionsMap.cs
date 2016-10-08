using System.Data.Entity.ModelConfiguration;
using Domain.Security;

namespace Persistence.Mappers
{ 
    public class ActionsMap : EntityTypeConfiguration<Actions>
    {
		public ActionsMap()
        {
            this.ToTable("Actionss");
            this.HasKey(actions => actions.Id);

            this.Property(actions => actions.Id).IsRequired();
		  
        }
	}
}