using System.Collections.Generic;
using Domain.Misc;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;
using Domain;
using System.Linq;
using Utils;

namespace Persistence.Dao
{ 
    public class UbicationDao : Dao<Ubication>, IUbicationDao
    {
		
		  public UbicationDao(IUnitOfWorkHelper unitOfWorkHelper, IActivatorWrapper activator)
            : base(unitOfWorkHelper, activator)
        {
        }

        public override IQueryable<Ubication> GetAllQ(string filtro)
        {
            var modelos = UnitOfWorkHelper.DBContext.Set<Ubication>().Where(d=>!d.IsDelete).AsQueryable();

            if (!string.IsNullOrEmpty(filtro))
                modelos = this.SetFiltro(modelos, filtro);

            return modelos.AsQueryable();
           // return base.GetAllQ(filtro);
        }

        public virtual IEnumerable<Ubication> GetAutoComplete(string text)
          {
              return
                  GetAll()
                      .Where(diagnostico => diagnostico.Description.ToLower().Contains(text.ToLower()))
                      .AsEnumerable().Take(10);
          }
		protected override IQueryable<Ubication> SetFiltro(IQueryable<Ubication> modelos, string filtro)
        {
            return modelos.Where(modelo => modelo.Description.ToLower().Contains(filtro.ToLower()) && !modelo.IsDelete);
        }
	}
}