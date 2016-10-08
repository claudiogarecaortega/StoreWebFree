using System;
using Domain.Contable;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;
using Domain;
using System.Linq;
using Utils;

namespace Persistence.Dao
{ 
    public class CuentasPagarDao : Dao<CuentasPagar>, ICuentasPagarDao
    {
		
		  public CuentasPagarDao(IUnitOfWorkHelper unitOfWorkHelper, IActivatorWrapper activator)
            : base(unitOfWorkHelper, activator)
        {
        }

        public int CuentasPagarCount()
        {
            return GetAll().Count(d => !d.Pagado && d.FechaVencimiento >= DateTime.Now.Date.AddDays(-3));
        }

        //protected override IQueryable<CuentasPagar> SetFiltro(IQueryable<CuentasPagar> modelos, string filtro)
        //{
        //    return modelos.Where(modelo => modelo.Description.ToLower().Contains(filtro.ToLower()));
        //}
	}
}