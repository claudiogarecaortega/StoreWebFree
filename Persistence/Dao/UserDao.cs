using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Users;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;
using Utils;

namespace Persistence.Dao
{
    public class UserDao: Dao<UserExtended>, IUserdDao
    {
        public UserDao(IUnitOfWorkHelper unitOfWorkHelper, IActivatorWrapper activator)
            : base(unitOfWorkHelper, activator)
        {
        }

        public IEnumerable<UserExtended> GetBornDates()
        {
            return base.GetAll()
                .Where(m => m.BornDate.Month == DateTime.Now.Month && m.BornDate.Day == DateTime.Now.Day);
        }

        public virtual IEnumerable<UserExtended> GetAutoComplete(string text)
        {
            return
                GetAll()
                    .Where(
                        diagnostico =>
                            diagnostico.PersonUser.LastName.ToLower().Contains(text.ToLower()) ||
                            diagnostico.PersonUser.Name.ToLower().Contains(text.ToLower()))
                    .AsEnumerable();
        }
    }
}
