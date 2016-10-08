using System.Collections.Generic;
using Domain.Security;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;
using Domain;
using System.Linq;
using Utils;

namespace Persistence.Dao
{ 
    public class ModuleDao : Dao<Module>, IModuleDao
    {
		
		  public ModuleDao(IUnitOfWorkHelper unitOfWorkHelper, IActivatorWrapper activator)
            : base(unitOfWorkHelper, activator)
        {
        }

          public override IQueryable<Module> GetAllQ(string filtro)
          {
              var modelos = UnitOfWorkHelper.DBContext.Set<Module>().Where(d => !d.IsDelete).AsQueryable();

              if (!string.IsNullOrEmpty(filtro))
                  modelos = this.SetFiltro(modelos, filtro);

              return modelos.AsQueryable();
          }
        public virtual IEnumerable<Module> GetAutoComplete(string text)
          {
              return
                  GetAll()
                      .Where(diagnostico => diagnostico.ModuleName.ToLower().Contains(text.ToLower()) && !diagnostico.IsDelete)
                      .AsEnumerable().Take(10);
          }
		protected override IQueryable<Module> SetFiltro(IQueryable<Module> modelos, string filtro)
        {
            return modelos.Where(modelo => modelo.ModuleName.ToLower().Contains(filtro.ToLower()) && !modelo.IsDelete);
        }
	}
}