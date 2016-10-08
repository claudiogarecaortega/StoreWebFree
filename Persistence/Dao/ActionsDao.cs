using Domain.Security;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;
using Domain;
using System.Linq;
using Domain.Misc;
using Utils;

namespace Persistence.Dao
{ 
    public class ActionsDao : Dao<Actions>, IActionsDao
    {
		
		  public ActionsDao(IUnitOfWorkHelper unitOfWorkHelper, IActivatorWrapper activator)
            : base(unitOfWorkHelper, activator)
        {
        }

        public override IQueryable<Actions> GetAllQ(string filtro)
        {
            var modelos = UnitOfWorkHelper.DBContext.Set<Actions>().Where(d => !d.IsDelete).AsQueryable();

            if (!string.IsNullOrEmpty(filtro))
                modelos = this.SetFiltro(modelos, filtro);

            return modelos.AsQueryable();
        }

        protected override IQueryable<Actions> SetFiltro(IQueryable<Actions> modelos, string filtro)
        {
            return modelos.Where(modelo => modelo.Description.ToLower().Contains(filtro.ToLower()));
        }
	}
}