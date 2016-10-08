using System;
using Domain.Ventas;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;
using Domain;
using System.Linq;
using Utils;

namespace Persistence.Dao
{ 
    public class VentaDao : Dao<Venta>, IVentaDao
    {
		
		  public VentaDao(IUnitOfWorkHelper unitOfWorkHelper, IActivatorWrapper activator)
            : base(unitOfWorkHelper, activator)
        {
        }

        public int VentasSemana()
        {
            return GetAll().Count(f => f.DateCreate >= DateTime.Now.Date.AddDays(-3));
        }

        protected override IQueryable<Venta> SetFiltro(IQueryable<Venta> modelos, string filtro)
        {
            return modelos.Where(modelo => modelo.Descripcion.ToLower().Contains(filtro.ToLower()));
        }
	}
}