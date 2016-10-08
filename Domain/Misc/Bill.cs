using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Domain.IdentificableObject;
using Domain.Users;

namespace Domain.Misc
{
    public class Bill : Audit, IIdentifiableObject
    {
        public int Id { get; set; }
        public string Description { get; set; }

    }
}
