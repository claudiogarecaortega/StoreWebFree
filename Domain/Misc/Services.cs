using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Clients;
using Domain.IdentificableObject;
using Domain.Users;

namespace Domain.Misc
{
    public class Services : Audit, IIdentifiableObject
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public virtual IList<Client> Serviceses { get; set; }
    }
}
