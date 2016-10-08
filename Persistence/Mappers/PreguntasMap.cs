using System.Data.Entity.ModelConfiguration;
using Domain.Products;

namespace Persistence.Mappers
{ 
    public class PreguntasMap : EntityTypeConfiguration<Preguntas>
    {
		public PreguntasMap()
        {
            this.ToTable("Preguntass");
            this.HasKey(preguntas => preguntas.Id);

            this.Property(preguntas => preguntas.Id).IsRequired();
        }
	}
}