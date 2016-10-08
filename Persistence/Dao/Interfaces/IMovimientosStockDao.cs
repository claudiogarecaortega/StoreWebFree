using System;
using System.Linq;
using Domain.Almacen;

namespace Persistence.Dao.Interfaces
{ 
    public interface IMovimientosStockDao : IDao<MovimientosStock>
    {
        IQueryable<MovimientosStock> GetAllAccount(int cuenta, bool infecha, DateTime start, DateTime end);
    }
}