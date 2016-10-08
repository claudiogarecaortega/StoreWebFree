using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Security;

namespace Persistence.Mappers
{
    public class ModuleActionsMap : EntityTypeConfiguration<ModuleActions>
    {
        public ModuleActionsMap()
        {
            this.ToTable("ModuleActions");
            this.HasKey(actions => actions.Id);

            this.Property(actions => actions.Id).IsRequired();
		    this.HasMany(m => m.Actions)
		        .WithMany(v=>v.ModuleActions)
                .Map(x => x.ToTable("ModuleActionMany").MapLeftKey("ModuleActionId").MapRightKey("ActionsId"));
            
        }
	}
}
