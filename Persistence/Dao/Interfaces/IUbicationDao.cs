using System.Collections.Generic;
using Domain.Misc;

namespace Persistence.Dao.Interfaces
{ 
    public interface IUbicationDao : IDao<Ubication>
    {
         IEnumerable<Ubication> GetAutoComplete(string text);
    }
}