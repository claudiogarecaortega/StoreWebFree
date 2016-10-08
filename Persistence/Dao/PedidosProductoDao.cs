using System;
using Domain.Providers;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;
using Domain;
using System.Linq;
using Utils;

namespace Persistence.Dao
{ 
    public class PedidosProductoDao : Dao<PedidosProducto>, IPedidosProductoDao
    {
		
		  public PedidosProductoDao(IUnitOfWorkHelper unitOfWorkHelper, IActivatorWrapper activator)
            : base(unitOfWorkHelper, activator)
        {
        }

        public int Pedidos()
        {
            return GetAll().Count(j => j.DateCreate >= DateTime.Now.Date.AddDays(-3));
        }

        protected override IQueryable<PedidosProducto> SetFiltro(IQueryable<PedidosProducto> modelos, string filtro)
        {
            return modelos.Where(modelo => modelo.Descripcion.ToLower().Contains(filtro.ToLower()));
        }
	}
}