using Domain.Misc;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;
using Domain;
using System.Linq;
using Utils;

namespace Persistence.Dao
{ 
    public class BillTypeDao : Dao<BillType>, IBillTypeDao
    {
		
		  public BillTypeDao(IUnitOfWorkHelper unitOfWorkHelper, IActivatorWrapper activator)
            : base(unitOfWorkHelper, activator)
        {
        }

        public override IQueryable<BillType> GetAllQ(string filtro)
          {
              var modelos = UnitOfWorkHelper.DBContext.Set<BillType>().Where(d => !d.IsDelete).AsQueryable();

              if (!string.IsNullOrEmpty(filtro))
                  modelos = this.SetFiltro(modelos, filtro);

              return modelos.AsQueryable();
            
        }

        protected override IQueryable<BillType> SetFiltro(IQueryable<BillType> modelos, string filtro)
        {
            return modelos.Where(modelo => modelo.Description.ToLower().Contains(filtro.ToLower()));
        }
	}
}