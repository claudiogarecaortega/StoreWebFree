using Domain.Misc;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;
using Domain;
using System.Linq;
using Utils;

namespace Persistence.Dao
{ 
    public class EtiquetasDao : Dao<Etiquetas>, IEtiquetasDao
    {
		
		  public EtiquetasDao(IUnitOfWorkHelper unitOfWorkHelper, IActivatorWrapper activator)
            : base(unitOfWorkHelper, activator)
        {
        }

		protected override IQueryable<Etiquetas> SetFiltro(IQueryable<Etiquetas> modelos, string filtro)
        {
            return modelos.Where(modelo => modelo.Descripcion.ToLower().Contains(filtro.ToLower()));
        }
	}
}