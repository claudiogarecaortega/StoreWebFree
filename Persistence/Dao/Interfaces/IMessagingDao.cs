using System.Collections.Generic;
using Domain.Misc;

namespace Persistence.Dao.Interfaces
{ 
    public interface IMessagingDao : IDao<Messaging>
    {
        IEnumerable<Messaging> GetAllUSer(int id);
        IEnumerable<Messaging> GetAllUSent(int id);
        IEnumerable<Messaging> GetAllUserUnread(int id);
    }
}