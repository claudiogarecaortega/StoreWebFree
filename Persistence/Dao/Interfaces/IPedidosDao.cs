using Domain.Ventas;

namespace Persistence.Dao.Interfaces
{ 
    public interface IPedidosDao : IDao<Pedidos>
    {
        int NuevosPedidos();
    }
}