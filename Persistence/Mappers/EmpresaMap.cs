using System.Data.Entity.ModelConfiguration;
using Domain.Misc;

namespace Persistence.Mappers
{ 
    public class EmpresaMap : EntityTypeConfiguration<Empresa>
    {
		public EmpresaMap()
        {
            this.ToTable("Empresas");
            this.HasKey(empresa => empresa.Id);

            this.Property(empresa => empresa.Id).IsRequired();
        }
	}
}