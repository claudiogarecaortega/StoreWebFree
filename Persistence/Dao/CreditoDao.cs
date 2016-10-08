using System;
using Domain.Contable;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;
using Domain;
using System.Linq;
using Utils;

namespace Persistence.Dao
{ 
    public class CreditoDao : Dao<Credito>, ICreditoDao
    {
		
		  public CreditoDao(IUnitOfWorkHelper unitOfWorkHelper, IActivatorWrapper activator)
            : base(unitOfWorkHelper, activator)
        {
        }

        public int GetCreditos()
        {
            return GetAll().Count(w => w.DateCreate >= DateTime.Now.Date.AddDays(-3));
        }

        protected override IQueryable<Credito> SetFiltro(IQueryable<Credito> modelos, string filtro)
        {
            return modelos.Where(modelo => modelo.Descripcion.ToLower().Contains(filtro.ToLower()));
        }
	}
}