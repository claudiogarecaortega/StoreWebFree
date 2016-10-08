using Domain.Misc;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;
using Domain;
using System.Linq;
using Utils;

namespace Persistence.Dao
{ 
    public class ServicesDao : Dao<Services>, IServicesDao
    {
		
		  public ServicesDao(IUnitOfWorkHelper unitOfWorkHelper, IActivatorWrapper activator)
            : base(unitOfWorkHelper, activator)
        {
        }
          public override IQueryable<Services> GetAllQ(string filtro)
          {
              var modelos = UnitOfWorkHelper.DBContext.Set<Services>().Where(d => !d.IsDelete).AsQueryable();

              if (!string.IsNullOrEmpty(filtro))
                  modelos = this.SetFiltro(modelos, filtro);

              return modelos.AsQueryable();
          }
		protected override IQueryable<Services> SetFiltro(IQueryable<Services> modelos, string filtro)
        {
            return modelos.Where(modelo => modelo.Description.ToLower().Contains(filtro.ToLower()) && !modelo.IsDelete);
        }
	}
}