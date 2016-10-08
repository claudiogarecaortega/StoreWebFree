using Domain.Contable;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;
using Domain;
using System.Linq;
using Utils;

namespace Persistence.Dao
{ 
    public class CuotasDao : Dao<Cuotas>, ICuotasDao
    {
		
		  public CuotasDao(IUnitOfWorkHelper unitOfWorkHelper, IActivatorWrapper activator)
            : base(unitOfWorkHelper, activator)
        {
        }

        //protected override IQueryable<Cuotas> SetFiltro(IQueryable<Cuotas> modelos, string filtro)
        //{
        //    return modelos.Where(modelo => modelo..ToLower().Contains(filtro.ToLower()));
        //}
	}
}