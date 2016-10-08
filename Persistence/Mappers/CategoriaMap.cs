using System.Data.Entity.ModelConfiguration;
using Domain.Misc;

namespace Persistence.Mappers
{ 
    public class CategoriaMap : EntityTypeConfiguration<Categoria>
    {
		public CategoriaMap()
        {
            this.ToTable("Categorias");
            this.HasKey(categoria => categoria.Id);

            this.Property(categoria => categoria.Id).IsRequired();
        }
	}
}