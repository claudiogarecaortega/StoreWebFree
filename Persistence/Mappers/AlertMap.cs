using System.Data.Entity.ModelConfiguration;
using Domain.Misc;

namespace Persistence.Mappers
{ 
    public class AlertMap : EntityTypeConfiguration<Alert>
    {
		public AlertMap()
        {
            this.ToTable("Alerts");
            this.HasKey(alert => alert.Id);

            this.Property(alert => alert.Id).IsRequired();
            this.HasMany(m=>m.UsersAlert)
                .WithRequired(c=>c.Alert)
                 .Map(cs =>
                 {
                     cs.MapKey("AlertId");
                     //cs.MapRightKey("AlerId");
                     //cs.ToTable("UserAlert");
                 });
        }
	}
}