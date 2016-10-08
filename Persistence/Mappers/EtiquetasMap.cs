using System.Data.Entity.ModelConfiguration;
using Domain.Misc;

namespace Persistence.Mappers
{ 
    public class EtiquetasMap : EntityTypeConfiguration<Etiquetas>
    {
		public EtiquetasMap()
        {
            this.ToTable("Etiquetass");
            this.HasKey(etiquetas => etiquetas.Id);

            this.Property(etiquetas => etiquetas.Id).IsRequired();
        }
	}
}