using Domain.Misc;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;
using Domain;
using System.Linq;
using Utils;

namespace Persistence.Dao
{ 
    public class TaxConditionDao : Dao<TaxCondition>, ITaxConditionDao
    {
		
		  public TaxConditionDao(IUnitOfWorkHelper unitOfWorkHelper, IActivatorWrapper activator)
            : base(unitOfWorkHelper, activator)
        {
        }
          public override IQueryable<TaxCondition> GetAllQ(string filtro)
          {
              var modelos = UnitOfWorkHelper.DBContext.Set<TaxCondition>().Where(d => !d.IsDelete).AsQueryable();

              if (!string.IsNullOrEmpty(filtro))
                  modelos = this.SetFiltro(modelos, filtro);

              return modelos.AsQueryable();
          }
		protected override IQueryable<TaxCondition> SetFiltro(IQueryable<TaxCondition> modelos, string filtro)
        {
            return modelos.Where(modelo => modelo.Description.ToLower().Contains(filtro.ToLower()) && !modelo.IsDelete);
        }
	}
}