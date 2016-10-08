using System.Data.Entity.ModelConfiguration;
using Domain.Security;

namespace Persistence.Mappers
{ 
    public class RolesMap : EntityTypeConfiguration<Roles>
    {
		public RolesMap()
        {
            this.ToTable("Roless");
            this.HasKey(roles => roles.Id);

            this.Property(roles => roles.Id).IsRequired();
		    this.HasMany(roles => roles.ListUser)
		        .WithRequired(roles => roles.UserRol)
		        .Map(x => x.MapKey("RoleId"));
		    this.HasMany(x => x.ListModulesActions)
		        .WithRequired(x => x.Role)
		        .Map(x => x.MapKey("RoleId"));

        }
	}
}