using System;
using Domain.Almacen;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;
using Domain;
using System.Linq;
using Domain.Products;
using Utils;

namespace Persistence.Dao
{ 
    public class MovimientosStockDao : Dao<MovimientosStock>, IMovimientosStockDao
    {
		
		  public MovimientosStockDao(IUnitOfWorkHelper unitOfWorkHelper, IActivatorWrapper activator)
            : base(unitOfWorkHelper, activator)
        {
        }
          public IQueryable<MovimientosStock> GetAllAccount(int cuenta, bool infecha, DateTime start, DateTime end)
          {
              var modelos = UnitOfWorkHelper.DBContext.Set<MovimientosStock>().Where(d => !d.IsDelete).AsQueryable();
              if (cuenta > 0)
                  modelos = UnitOfWorkHelper.DBContext.Set<MovimientosStock>().Where(d => !d.IsDelete && d.Stock.Producto.Id==cuenta).AsQueryable();
              if (infecha)
                  modelos = modelos.Where(d => d.DateCreate >= start && d.DateCreate <= end);

              return modelos.AsQueryable();
              //return base.GetAll().AsQueryable();
          }
		protected override IQueryable<MovimientosStock> SetFiltro(IQueryable<MovimientosStock> modelos, string filtro)
        {
            return modelos.Where(modelo => modelo.Descripcion.ToLower().Contains(filtro.ToLower()));
        }
	}
}