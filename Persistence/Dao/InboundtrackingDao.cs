using Domain.Commodity;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;
using Domain;
using System.Linq;
using Utils;

namespace Persistence.Dao
{ 
    public class InboundtrackingDao : Dao<InboundTracking>, IInboundtrackingDao
    {
		
		  public InboundtrackingDao(IUnitOfWorkHelper unitOfWorkHelper, IActivatorWrapper activator)
            : base(unitOfWorkHelper, activator)
        {
        }
          public override IQueryable<InboundTracking> GetAllQ(string filtro)
          {
              var modelos = UnitOfWorkHelper.DBContext.Set<InboundTracking>().Where(d => !d.IsDelete).AsQueryable();

              if (!string.IsNullOrEmpty(filtro))
                  modelos = this.SetFiltro(modelos, filtro);

              return modelos.AsQueryable();
          }
          protected override IQueryable<InboundTracking> SetFiltro(IQueryable<InboundTracking> modelos, string filtro)
        {
            return modelos.Where(modelo => modelo.Ubication.Description.ToLower().Contains(filtro.ToLower()) && !modelo.IsDelete);
        }
	}
}