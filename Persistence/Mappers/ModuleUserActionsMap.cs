using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Security;

namespace Persistence.Mappers
{
    public class ModuleUserActionsMap : EntityTypeConfiguration<ModuleUserActions>
    {
        public ModuleUserActionsMap()
        {
            this.ToTable("ModuleUserActions");
            this.HasKey(actions => actions.Id);

            this.Property(actions => actions.Id).IsRequired();
            this.HasMany(m => m.Actions)
                 .WithMany(v => v.ModuleUserActions)
                 .Map(x => x.ToTable("ModuleActionManyUser").MapLeftKey("ModuleActionUserId").MapRightKey("ActionsId"));
            }
	}
}
