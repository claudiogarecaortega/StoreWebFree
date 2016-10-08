using Domain.Providers;

namespace Persistence.Dao.Interfaces
{ 
    public interface IPedidosProductoDao : IDao<PedidosProducto>
    {
        int Pedidos();
    }
}