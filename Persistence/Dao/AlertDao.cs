using System.Collections.Generic;
using Domain.Misc;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;
using Domain;
using System.Linq;
using Utils;

namespace Persistence.Dao
{ 
    public class AlertDao : Dao<Alert>, IAlertDao
    {
		
		  public AlertDao(IUnitOfWorkHelper unitOfWorkHelper, IActivatorWrapper activator)
            : base(unitOfWorkHelper, activator)
        {
        }

        public override IQueryable<Alert> GetAllQ(string filtro)
        {
            var modelos = UnitOfWorkHelper.DBContext.Set<Alert>().Where(d => !d.IsDelete).AsQueryable();

            if (!string.IsNullOrEmpty(filtro))
                modelos = this.SetFiltro(modelos, filtro);

            return modelos.AsQueryable();
            
        }

        public IEnumerable<Alert> GetAlertsByUser(int id)
          {
              var sa = base.GetAll().Where(s => (s.IsForAll || s.UsersAlert.Select(r => r.User.Id).Contains(id) ));
             var a = sa.Select(e => e.UsersAlert.Select(d => !d.IsRead));
              return sa;
          }
		protected override IQueryable<Alert> SetFiltro(IQueryable<Alert> modelos, string filtro)
        {
            return modelos.Where(modelo => modelo.Description.ToLower().Contains(filtro.ToLower()));
        }
	}
}