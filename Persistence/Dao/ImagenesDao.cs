using Domain.Products;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;
using Domain;
using System.Linq;
using Utils;

namespace Persistence.Dao
{ 
    public class ImagenesDao : Dao<Imagenes>, IImagenesDao
    {
		
		  public ImagenesDao(IUnitOfWorkHelper unitOfWorkHelper, IActivatorWrapper activator)
            : base(unitOfWorkHelper, activator)
        {
        }

		protected override IQueryable<Imagenes> SetFiltro(IQueryable<Imagenes> modelos, string filtro)
        {
            return modelos.Where(modelo => modelo.Descripcion.ToLower().Contains(filtro.ToLower()));
        }
	}
}