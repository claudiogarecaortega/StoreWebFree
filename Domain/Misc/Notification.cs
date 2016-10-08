using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.IdentificableObject;
using Domain.Users;

namespace Domain.Misc
{
    public class Notification : Audit, IIdentifiableObject
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
        public int IdProduct { get; set; }
        //public string Module { get; set; }
        //public string Controller { get; set; }
        public string Importance { get; set; }
        public bool IsForAllUSers { get; set; }
        public bool IsRead { get; set; }

        public virtual IList<UserNotification> UserToNotifiy { get; set; }
     }
}
