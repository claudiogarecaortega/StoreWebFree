using System.Collections.Generic;
using Domain.Security;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;
using Domain;
using System.Linq;
using Utils;

namespace Persistence.Dao
{ 
    public class RolesDao : Dao<Roles>, IRolesDao
    {
		
		  public RolesDao(IUnitOfWorkHelper unitOfWorkHelper, IActivatorWrapper activator)
            : base(unitOfWorkHelper, activator)
        {
        }
          public override IQueryable<Roles> GetAllQ(string filtro)
          {
              var modelos = UnitOfWorkHelper.DBContext.Set<Roles>().Where(d => !d.IsDelete).AsQueryable();

              if (!string.IsNullOrEmpty(filtro))
                  modelos = this.SetFiltro(modelos, filtro);

              return modelos.AsQueryable();
          }
        public IEnumerable<Module> GetModulesRole(int id)
        {
            var firstOrDefault = GetAll().FirstOrDefault(c => c.Id == id);
            if (firstOrDefault != null)
            {
                var modules=firstOrDefault.ListModulesActions.Select(x => x.Module);
                return modules;
            }
            return null;
            //if (firstOrDefault != null)
            //    return firstOrDefault.ListModulesActions;
        }

        protected override IQueryable<Roles> SetFiltro(IQueryable<Roles> modelos, string filtro)
        {
            return modelos.Where(modelo => modelo.Description.ToLower().Contains(filtro.ToLower()) && !modelo.IsDelete);
        }
	}
}