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
    public class Contract : Audit, IIdentifiableObject
    {
        public int  Id { get; set; }
        public string Description  { get; set; }
        public string Contrato  { get; set; }
        public virtual Client Client { get; set; }
    }
}
