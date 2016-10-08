using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.IdentificableObject;
using Domain.Users;

namespace Domain.Misc
{
    public class Alert : Audit, IIdentifiableObject
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsForAll { get; set; }
        public int Importance { get; set; }
        public bool ShowHome { get; set; }
        public virtual IList<UserAlert> UsersAlert { get; set; }
        public bool IsHtml { get; set; }
        public string Html { get; set; }

    }
}
