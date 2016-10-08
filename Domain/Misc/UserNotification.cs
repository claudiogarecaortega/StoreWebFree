using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Users;

namespace Domain.Misc
{
    public class UserNotification
    {

        public int Id { get; set; }
        public virtual UserExtended User { get; set; }
        public virtual Notification Alert { get; set; }
        public bool IsRead { get; set; }
    }
}
