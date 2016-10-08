using System.Data.Entity.ModelConfiguration;
using Domain.Products;

namespace Persistence.Mappers
{ 
    public class ComentariosMap : EntityTypeConfiguration<Comentarios>
    {
		public ComentariosMap()
        {
            this.ToTable("Comentarioss");
            this.HasKey(comentarios => comentarios.Id);

            this.Property(comentarios => comentarios.Id).IsRequired();
        }
	}
}