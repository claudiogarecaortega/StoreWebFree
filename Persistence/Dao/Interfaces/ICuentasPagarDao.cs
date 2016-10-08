using Domain.Contable;

namespace Persistence.Dao.Interfaces
{ 
    public interface ICuentasPagarDao : IDao<CuentasPagar>
    {
        int CuentasPagarCount();
    }
}