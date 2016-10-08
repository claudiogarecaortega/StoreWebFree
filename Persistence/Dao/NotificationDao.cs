using System.Collections.Generic;
using Domain.Misc;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;
using Domain;
using System.Linq;
using Domain.Security;
using Utils;

namespace Persistence.Dao
{ 
    public class NotificationDao : Dao<Notification>, INotificationDao
    {
		
		  public NotificationDao(IUnitOfWorkHelper unitOfWorkHelper, IActivatorWrapper activator)
            : base(unitOfWorkHelper, activator)
        {
        }
          public override IQueryable<Notification> GetAllQ(string filtro)
          {
              var modelos = UnitOfWorkHelper.DBContext.Set<Notification>().Where(d => !d.IsDelete).AsQueryable();

              if (!string.IsNullOrEmpty(filtro))
                  modelos = this.SetFiltro(modelos, filtro);

              return modelos.AsQueryable();
          }
        public IEnumerable<Notification> GetNotificationByUser(int id)
        {
           // return base.GetAll().Where(s => s.IsForAllUSers || s.UserToNotifiy.Select(r=>r.User.Id).Contains(id) && !s.IsRead&&!s.IsDelete);
            var Notificationes = base.GetAll().Where(s => s.IsForAllUSers || s.UserToNotifiy.Select(r => r.User.Id).Contains(id) && !s.IsRead && !s.IsDelete);
            var listaNotifications = new List<Notification>();
            foreach (var alert in Notificationes)
            {
                if (alert.UserToNotifiy.Any(f => !f.IsRead && f.User.Id == id))
                    listaNotifications.Add(alert);

            }
            return listaNotifications;
        }

        protected override IQueryable<Notification> SetFiltro(IQueryable<Notification> modelos, string filtro)
        {
            return modelos.Where(modelo => modelo.Message.ToLower().Contains(filtro.ToLower()) && !modelo.IsDelete);
        }
	}
}