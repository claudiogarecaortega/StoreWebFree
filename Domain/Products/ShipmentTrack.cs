using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.IdentificableObject;
using Domain.Misc;
using Domain.Users;

namespace Domain.Products
{
    public class ShipmentTrack : Audit, IIdentifiableObject
    {
        public int Id { get; set; }
        public string Observaciones { get; set; }
        public virtual Ubication Ubication { get; set; }
        public DateTime DateTrack { get; set; }
        public bool IsWarning { get; set; }
        public bool IsReceived { get; set; }
        public bool IsSended { get; set; }
        public virtual Shipment Shipment { get; set; }
        //public UserExtended User { get; set; }
    }
}
