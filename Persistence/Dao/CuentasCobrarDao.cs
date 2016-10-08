using System;
using Domain.Contable;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;
using Domain;
using System.Linq;
using Utils;

namespace Persistence.Dao
{ 
    public class CuentasCobrarDao : Dao<CuentasCobrar>, ICuentasCobrarDao
    {
		
		  public CuentasCobrarDao(IUnitOfWorkHelper unitOfWorkHelper, IActivatorWrapper activator)
            : base(unitOfWorkHelper, activator)
        {
        }
          public int CuentasCobrarCount()
          {
              return GetAll().Count(d => !d.Cobrado && d.FechaVencimiento >= DateTime.Now.Date.AddDays(-3));
          }
        //protected override IQueryable<CuentasCobrar> SetFiltro(IQueryable<CuentasCobrar> modelos, string filtro)
        //{
        //    return modelos.Where(modelo => modelo.Monto.ToLower().Contains(filtro.ToLower()));
        //}
	}
}