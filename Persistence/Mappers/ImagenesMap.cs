using System.Data.Entity.ModelConfiguration;
using Domain.Products;

namespace Persistence.Mappers
{ 
    public class ImagenesMap : EntityTypeConfiguration<Imagenes>
    {
		public ImagenesMap()
        {
            this.ToTable("Imageness");
            this.HasKey(imagenes => imagenes.Id);

            this.Property(imagenes => imagenes.Id).IsRequired();
        }
	}
}