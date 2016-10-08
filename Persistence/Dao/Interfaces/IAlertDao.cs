using System.Collections.Generic;
using Domain.Misc;

namespace Persistence.Dao.Interfaces
{ 
    public interface IAlertDao : IDao<Alert>
    {
        IEnumerable<Alert> GetAlertsByUser(int id);
    }
}