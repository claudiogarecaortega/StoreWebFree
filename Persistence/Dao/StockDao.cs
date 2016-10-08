using Domain.Almacen;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;
using Domain;
using System.Linq;
using Utils;

namespace Persistence.Dao
{ 
    public class StockDao : Dao<Stock>, IStockDao
    {
		
		  public StockDao(IUnitOfWorkHelper unitOfWorkHelper, IActivatorWrapper activator)
            : base(unitOfWorkHelper, activator)
        {
        }

        //protected override IQueryable<Stock> SetFiltro(IQueryable<Stock> modelos, string filtro)
        //{
        //    return modelos.Where(modelo => modelo..ToLower().Contains(filtro.ToLower()));
        //}
	}
}