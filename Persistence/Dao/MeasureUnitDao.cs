using Domain.Misc;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;
using Domain;
using System.Linq;
using Utils;

namespace Persistence.Dao
{ 
    public class MeasureUnitDao : Dao<MeasureUnit>, IMeasureUnitDao
    {
		
		  public MeasureUnitDao(IUnitOfWorkHelper unitOfWorkHelper, IActivatorWrapper activator)
            : base(unitOfWorkHelper, activator)
        {
        }
          public override IQueryable<MeasureUnit> GetAllQ(string filtro)
          {
              var modelos = UnitOfWorkHelper.DBContext.Set<MeasureUnit>().Where(d => !d.IsDelete).AsQueryable();

              if (!string.IsNullOrEmpty(filtro))
                  modelos = this.SetFiltro(modelos, filtro);

              return modelos.AsQueryable();
          }

		protected override IQueryable<MeasureUnit> SetFiltro(IQueryable<MeasureUnit> modelos, string filtro)
        {
            return modelos.Where(modelo => modelo.Description.ToLower().Contains(filtro.ToLower()) && !modelo.IsDelete);
        }
	}
}