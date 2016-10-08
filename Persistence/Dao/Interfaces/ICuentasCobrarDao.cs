using Domain.Contable;

namespace Persistence.Dao.Interfaces
{ 
    public interface ICuentasCobrarDao : IDao<CuentasCobrar>
    {
        int CuentasCobrarCount();
    }
}