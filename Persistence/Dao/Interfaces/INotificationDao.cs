using System.Collections.Generic;
using Domain.Misc;

namespace Persistence.Dao.Interfaces
{ 
    public interface INotificationDao : IDao<Notification>
    {
        IEnumerable<Notification> GetNotificationByUser(int id);
    }
}