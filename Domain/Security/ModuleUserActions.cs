using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Users;

namespace Domain.Security
{
    public class ModuleUserActions
    {
        public int Id { get; set; }
        public virtual Module Module { get; set; }
        public virtual IList<Actions> Actions { get; set; }
        public virtual UserExtended User { get; set; }
        public bool IsActive { get; set; }
    }

}
