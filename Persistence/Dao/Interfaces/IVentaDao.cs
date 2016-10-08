using Domain.Ventas;

namespace Persistence.Dao.Interfaces
{ 
    public interface IVentaDao : IDao<Venta>
    {
         int VentasSemana();
    }
}