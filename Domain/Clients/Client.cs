using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contable;
using Domain.IdentificableObject;
using Domain.Misc;
using Domain.Users;
using Domain.Ventas;

namespace Domain.Clients
{
    public class Client : Audit, IIdentifiableObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public decimal Sequence { get; set; }
        public long ZipCode { get; set; }
        //public string Contact { get; set; }
        public string NameContact { get; set; }
        public string MailContact { get; set; }
        public string PhoneContact { get; set; }
        public string NameContact2 { get; set; }
        public string MailContact2 { get; set; }
        public string PhoneContact2 { get; set; }
        //public string MeasureUnit  { get; set; }
        public string DeliveryDirection  { get; set; }
        public string Neighborhood { get; set; }
        public string Direction { get; set; }
        public string Localidad { get; set; }
        //public string TimeToDelivery { get; set; }
        //public string TimeToDeliveryMorningStart { get; set; }
        //public string TimeToDeliveryMorningEnd { get; set; }
        //public string TimeToDeliveryAftermonStart { get; set; }
        //public string TimeToDeliveryAftermonEnd { get; set; }
       // public double? ServicePrice { get; set; }
       // public string IiBb { get; set; }
       // public string Cuit { get; set; }
        public string DocumentoIdentidad { get; set; }
        public string Description { get; set; }
        //public bool IsClientDestination { get; set; }
        //public bool IsClientOrigen { get; set; }
        public virtual IList<Credito> Creditos { get; set; }
        public virtual IList<Venta> Ventas { get; set; }
        public virtual Ubication Ubication { get; set; }
        //public virtual IList<Services> Serviceses { get; set; }
        //public virtual UserExtended Seller { get; set; }
        //public virtual TaxCondition TaxCondition { get; set; }
        public virtual Contract Contract { get; set; }

        public string GetCompleteName()
        {
            return this.Name;
        }
    }
}
