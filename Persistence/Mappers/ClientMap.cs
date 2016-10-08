using System.Data.Entity.ModelConfiguration;
using Domain.Clients;

namespace Persistence.Mappers
{ 
    public class ClientMap : EntityTypeConfiguration<Client>
    {
		public ClientMap()
        {
            this.ToTable("Clients");
            this.HasKey(client => client.Id);

            this.Property(client => client.Id).IsRequired();
            this.HasOptional(s => s.Contract)
		        .WithRequired(r => r.Client)
		        .Map(c => c.MapKey("ClientId"));
        }
	}
}