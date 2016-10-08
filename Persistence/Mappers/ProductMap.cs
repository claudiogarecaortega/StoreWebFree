using System.Data.Entity.ModelConfiguration;
using Domain.Products;

namespace Persistence.Mappers
{ 
    public class ProductMap : EntityTypeConfiguration<Product>
    {
		public ProductMap()
        {
            this.ToTable("Products");
            this.HasKey(product => product.Id);

            this.Property(product => product.Id).IsRequired();
		    this.HasMany(c => c.Categorias)
		        .WithMany(f => f.Productos)
		        .Map(z =>
		        {
		            z.ToTable("ProductsCategoria");
		            z.MapLeftKey("ProductoId");
		            z.MapRightKey("CategoriaId");
		        });
            this.HasMany(c => c.Etiquetas)
                .WithMany(f => f.Productos)
                .Map(z =>
                {
                    z.ToTable("ProductsEtiquetas");
                    z.MapLeftKey("ProductoId");
                    z.MapRightKey("EtiquetasId");
                });
        }
	}
}