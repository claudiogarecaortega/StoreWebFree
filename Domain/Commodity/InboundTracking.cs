using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.IdentificableObject;
using Domain.Misc;
using Domain.Users;

namespace Domain.Commodity
{
    public class InboundTracking : Audit, IIdentifiableObject
    {
        public int Id { get; set; }
        public Inbound Inbound { get; set; }
        public Ubication Ubication { get; set; }
        public DateTime DateTrack { get; set; }
        
    }
}
