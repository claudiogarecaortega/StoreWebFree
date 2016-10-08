using System;
using System.Linq;
using Domain.Products;

namespace Persistence.Dao.Interfaces
{ 
    public interface IShipmentDao : IDao<Shipment>
    {
        IQueryable<Shipment> GetAllQFiltros(string filtro, bool viaje, bool end, bool init);
        IQueryable<Shipment> GetAllAccount(int cuenta, bool infecha, DateTime start, DateTime end);
    }
}