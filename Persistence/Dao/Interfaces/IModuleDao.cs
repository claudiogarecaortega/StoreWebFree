using System.Collections.Generic;
using Domain.Security;

namespace Persistence.Dao.Interfaces
{ 
    public interface IModuleDao : IDao<Module>
    {
        IEnumerable<Module> GetAutoComplete(string text);
    }
}