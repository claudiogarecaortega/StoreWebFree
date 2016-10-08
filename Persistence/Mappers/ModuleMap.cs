using System.Data.Entity.ModelConfiguration;
using Domain.Security;

namespace Persistence.Mappers
{ 
    public class ModuleMap : EntityTypeConfiguration<Module>
    {
		public ModuleMap()
        {
            this.ToTable("Modules");
            this.HasKey(module => module.Id);

            this.Property(module => module.Id).IsRequired();
		    this.HasMany(x => x.ListRoles)
		        .WithRequired(m => m.Module)
		        .Map(x => x.MapKey("ModuleId"));
		    this.HasMany(x => x.ListUserModules)
		        .WithOptional(x => x.Module)
		        .Map(z => z.MapKey("ModuleId"));

        }
	}
}