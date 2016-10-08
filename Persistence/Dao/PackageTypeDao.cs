using Domain.Misc;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;
using Domain;
using System.Linq;
using Utils;

namespace Persistence.Dao
{ 
    public class PackageTypeDao : Dao<PackageType>, IPackageTypeDao
    {
		
		  public PackageTypeDao(IUnitOfWorkHelper unitOfWorkHelper, IActivatorWrapper activator)
            : base(unitOfWorkHelper, activator)
        {
        }
          public override IQueryable<PackageType> GetAllQ(string filtro)
          {
              var modelos = UnitOfWorkHelper.DBContext.Set<PackageType>().Where(d => !d.IsDelete).AsQueryable();

              if (!string.IsNullOrEmpty(filtro))
                  modelos = this.SetFiltro(modelos, filtro);

              return modelos.AsQueryable();
          }

		protected override IQueryable<PackageType> SetFiltro(IQueryable<PackageType> modelos, string filtro)
        {
            return modelos.Where(modelo => modelo.Description.ToLower().Contains(filtro.ToLower()) && !modelo.IsDelete);
        }
	}
}