using Domain.Contable;

namespace Persistence.Dao.Interfaces
{ 
    public interface ICreditoDao : IDao<Credito>
    {
        int GetCreditos();
    }
}