using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Users;

namespace Persistence.Dao.Interfaces
{
    public interface IUserdDao : IDao<UserExtended>
    {
        IEnumerable<UserExtended> GetAutoComplete(string text);
        IEnumerable<UserExtended> GetBornDates();
    }
}
