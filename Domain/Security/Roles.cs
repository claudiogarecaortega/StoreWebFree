using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Users;
using Microsoft.VisualBasic.ApplicationServices;

namespace Domain.Security
{
    public class Roles : Audit
    {
        public int  Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual  IList<ModuleActions> ListModulesActions { get; set; }
        public virtual  IList<UserExtended> ListUser { get; set; }
    }
}
