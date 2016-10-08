using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Users;
using Microsoft.VisualBasic.ApplicationServices;

namespace Domain.Security
{
    public class Module : Audit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ModuleName { get; set; }
        public virtual Module ModuleParent { get; set; }
        //public virtual  IList<Action> Actions { get; set; }
        public virtual  IList<ModuleActions> ListRoles { get; set; }
        public virtual  IList<ModuleUserActions> ListUserModules { get; set; }
       // public virtual  IList<User> ListUsers { get; set; }
    }
}
