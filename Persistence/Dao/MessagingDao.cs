using System.Collections.Generic;
using Domain.Misc;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;
using Domain;
using System.Linq;
using Utils;

namespace Persistence.Dao
{ 
    public class MessagingDao : Dao<Messaging>, IMessagingDao
    {
		
		  public MessagingDao(IUnitOfWorkHelper unitOfWorkHelper, IActivatorWrapper activator)
            : base(unitOfWorkHelper, activator)
        {
        }
        
        public IEnumerable<Messaging> GetAllUSer(int id)
        {
            return base.GetAll().Where(c => c.UserReciver.Any(s=>s.Id== id)).OrderByDescending(f=>f.DateSend);
        }
        public IEnumerable<Messaging> GetAllUserUnread(int id)
        {
            return base.GetAll().Where(c => c.UserReciver.Any(s => s.Id == id) && !c.IsRead);
        }
        public IEnumerable<Messaging> GetAllUSent(int id)
        {
            return base.GetAll().Where(c => c.UserSend.Id == id);
        }

        protected override IQueryable<Messaging> SetFiltro(IQueryable<Messaging> modelos, string filtro)
        {
            return modelos.Where(modelo => modelo.Message.ToLower().Contains(filtro.ToLower()) && !modelo.IsDelete);
        }
	}
}