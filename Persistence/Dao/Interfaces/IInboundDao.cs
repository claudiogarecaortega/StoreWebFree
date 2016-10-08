using System.Collections.Generic;
using System.Linq;
using Domain.Commodity;

namespace Persistence.Dao.Interfaces
{ 
    public interface IInboundDao : IDao<Inbound>
    {
        IEnumerable<Inbound> GetAllFilter(int[] id, bool edit);
        IQueryable<Inbound> GetAllQFiltros(string filtro, bool viaje, bool end, bool init);
        IQueryable<Inbound> GetAllQPlus(string filtro);
        IQueryable<Inbound> GetAllQRestore(string filtro);
        IQueryable<Inbound> GetAllQLess(string filtro, string filtro2, bool viaje, bool end, bool init);
    }
}