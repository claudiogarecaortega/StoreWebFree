using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Commodity;
using Domain.IdentificableObject;
using Domain.Misc;
using Domain.Users;

namespace Domain.Products
{
    public class Shipment : Audit, IIdentifiableObject
    {
        public int Id { get; set; }
        public decimal TotalKilos { get; set; }
        public long TotalPakages { get; set; }
        public virtual IList<Inbound> Cargars { get; set; }
        public virtual Ubication UbicationFrom { get; set; }
        public virtual Ubication UbicationTo { get; set; }
        public virtual Car Truck { get; set; }
        public virtual IList<Ubication> UbicationRoute { get; set; }
        public bool IsDecline { get; set; }
        public bool IsSent { get; set; }
        public bool IsTraveling { get; set; }
        public bool IsFinishig { get; set; }
        public string Observations { get; set; }
        public virtual IList<ShipmentTrack> Tracks { get; set; }

        public string Status()
        {
            
                if (IsFinishig)
                    return "green";
                if (IsTraveling || IsSent)
                    return "yellow";
                if (IsDecline)
                    return "red";

            
            return "white";
        }
    }
}
