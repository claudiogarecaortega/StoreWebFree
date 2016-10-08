using Domain.Providers;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;
using Domain;
using System.Linq;
using Utils;

namespace Persistence.Dao
{ 
    public class ProviderDao : Dao<Provider>, IProviderDao
    {
		
		  public ProviderDao(IUnitOfWorkHelper unitOfWorkHelper, IActivatorWrapper activator)
            : base(unitOfWorkHelper, activator)
        {
        }
          public override IQueryable<Provider> GetAllQ(string filtro)
          {
              var modelos = UnitOfWorkHelper.DBContext.Set<Provider>().Where(d => !d.IsDelete).AsQueryable();

              if (!string.IsNullOrEmpty(filtro))
                  modelos = this.SetFiltro(modelos, filtro);

              return modelos.AsQueryable();
          }
		protected override IQueryable<Provider> SetFiltro(IQueryable<Provider> modelos, string filtro)
        {
            return modelos.Where(modelo => modelo.Description.ToLower().Contains(filtro.ToLower()) && !modelo.IsDelete);
        }
	}
}