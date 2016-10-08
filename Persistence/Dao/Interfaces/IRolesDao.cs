using System.Collections.Generic;
using Domain.Security;

namespace Persistence.Dao.Interfaces
{ 
    public interface IRolesDao : IDao<Roles>
    {
        IEnumerable<Module> GetModulesRole(int id);
    }
}