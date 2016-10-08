using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Misc;
using Domain.Products;
using Domain.Users;
using Domain.Clients;
using Domain.IdentificableObject;

namespace Domain.Commodity
{
    public class Inbound : Audit, IIdentifiableObject
    {
        public int Id { get; set; }
        public DateTime DateIn { get; set; }
        public DateTime DateTicket { get; set; }
        public string BillNumber { get; set; }
        public decimal UsoFisico { get; set; }
        public decimal PriceDecimal { get; set; }
        public decimal Quantity { get; set; }
        public bool IsCold { get; set; }
        public string Description { get; set; }
        public string Kilos { get; set; }
        public bool IsUsed { get; set; }
        public bool IsDelivered { get; set; }
        public bool IsDecline { get; set; }
        public virtual UserExtended UserControl { get; set; }
        public virtual Client ClientTo { get; set; }
        public virtual Client ClientFrom { get; set; }
        public virtual Client ClientOrigen { get; set; }
        public virtual MeasureUnit MeasureUnit { get; set; }
        public virtual PackageType PackageType { get; set; }
        public virtual Product Product { get; set; }
        public virtual Bill Bill { get; set; }
        public virtual BillType BillType { get; set; }
        public virtual Shipment Envio { get; set; }

        public string Status()
        {
           if (Envio != null)
            {
                if (Envio.IsFinishig||IsDelivered)
                    return "green";
                if (Envio.IsTraveling || Envio.IsSent|| IsUsed)
                    return "yellow";

            }
            return "white";
        }

        public string UbicationDescription()
        {
            return "Ingreso N°:  " + this.Secuencia+ "  Destino: " + this.ClientTo.Alias;
        }
    }
}
