using System.Data.Entity.ModelConfiguration;
using Domain.Misc;

namespace Persistence.Mappers
{ 
    public class MessagingMap : EntityTypeConfiguration<Messaging>
    {
		public MessagingMap()
        {
            this.ToTable("Messagings");
            this.HasKey(messaging => messaging.Id);
            Property(x => x.DateRead);
            Property(x => x.DateSend);
            Property(x => x.IsRead);
            Property(x => x.IsReciper);
            Property(x => x.IsSend);
            Property(x => x.IsUrgent);
            Property(x => x.Message);

            this.Property(messaging => messaging.Id).IsRequired();
            HasOptional(x => x.UserSend).WithMany(x => x.MessagingsSend).Map(x => x.MapKey("UserSendId"));
            HasMany(x => x.UserReciver).WithMany(x => x.MessagingsInbox).Map(cs =>
            {
                cs.MapLeftKey("UserId");
                cs.MapRightKey("MessageId");
                cs.ToTable("MessageUsers");
            });
            HasOptional(x => x.MessagingParent).WithMany().Map(x => x.MapKey("MessagingId"));
        }
	}
}