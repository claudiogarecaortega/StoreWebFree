using Domain.Ventas;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;
using Domain;
using System.Linq;
using Utils;

namespace Persistence.Dao
{ 
    public class PedidosDao : Dao<Pedidos>, IPedidosDao
    {
		
		  public PedidosDao(IUnitOfWorkHelper unitOfWorkHelper, IActivatorWrapper activator)
            : base(unitOfWorkHelper, activator)
        {
        }

        public int NuevosPedidos()
        {
            return GetAll().Count(d => !d.Cumplido);
        }

        //protected override IQueryable<Pedidos> SetFiltro(IQueryable<Pedidos> modelos, string filtro)
        //{
        //    return modelos.Where(modelo => modelo.Description.ToLower().Contains(filtro.ToLower()));
        //}
	}
}