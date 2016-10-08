using Domain.Misc;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;
using Domain;
using System.Linq;
using Utils;

namespace Persistence.Dao
{ 
    public class BillDao : Dao<Bill>, IBillDao
    {
		
		  public BillDao(IUnitOfWorkHelper unitOfWorkHelper, IActivatorWrapper activator)
            : base(unitOfWorkHelper, activator)
        {
        }

        public override IQueryable<Bill> GetAllQ(string filtro)
        {
            var modelos = UnitOfWorkHelper.DBContext.Set<Bill>().Where(d => !d.IsDelete).AsQueryable();

            if (!string.IsNullOrEmpty(filtro))
                modelos = this.SetFiltro(modelos, filtro);

            return modelos.AsQueryable();
            
        }

        protected override IQueryable<Bill> SetFiltro(IQueryable<Bill> modelos, string filtro)
        {
            return modelos.Where(modelo => modelo.Description.ToLower().Contains(filtro.ToLower()));
        }
	}
}