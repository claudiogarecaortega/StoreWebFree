using System.Data.Entity.ModelConfiguration;
using Domain.Misc;

namespace Persistence.Mappers
{ 
    public class NotificationMap : EntityTypeConfiguration<Notification>
    {
		public NotificationMap()
        {
            this.ToTable("Notifications");
            this.HasKey(notification => notification.Id);

            this.Property(notification => notification.Id).IsRequired();
            this.HasMany(m => m.UserToNotifiy)
              .WithRequired(c => c.Alert)
               .Map(cs =>
               {
                   cs.MapKey("NotificationId");
                   //cs.MapRightKey("AlerId");
                   //cs.ToTable("UserAlert");
               });
           
        }
	}
}