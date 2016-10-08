using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.IdentificableObject;
using Domain.Products;
using Domain.Users;

namespace Domain.Misc
{
    public class Car : Audit, IIdentifiableObject
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Description { get; set; }
        public string Patente { get; set; }
        public string Senasa { get; set; }
        public string Senada { get; set; }
        public string Seguro { get; set; }
        public string Poliza { get; set; }
        public DateTime DueDate { get; set; }
        public decimal UsoFisico { get; set; }
        public decimal KiloMaximo { get; set; }
        public decimal PalletMaximo { get; set; }
        public bool IsTraveling { get; set; }
        public bool IsMaintenace { get; set; }
        public string UrgenciPhone { get; set; }
        public string CarPhone { get; set; }
        public string Itv { get; set; }
        public string Itvtest { get; set; }
        public DateTime DueDateItv { get; set; }
        public virtual IList<Shipment> Shipments { get; set; }

        public Shipment GetCurrentTrip()
        {
            return Shipments.FirstOrDefault(r => r.IsSent && r.IsTraveling && !r.IsDecline);
        }
    }
}
