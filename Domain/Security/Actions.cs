using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Users;

namespace Domain.Security
{
    public class Actions : Audit
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public virtual IList<ModuleActions> ModuleActions{ get; set; }
        public virtual IList<ModuleUserActions>ModuleUserActions{ get; set; }
        
    }
}
