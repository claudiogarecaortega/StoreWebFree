using Domain.Contable;

namespace Persistence.Dao.Interfaces
{ 
    public interface IBancariaDao : IDao<Bancaria>
    {
        Bancaria GetCuentaIgresos();
        Bancaria GetCuentaEgresos();
    }
}