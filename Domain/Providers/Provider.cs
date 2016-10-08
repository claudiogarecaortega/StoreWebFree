using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.IdentificableObject;
using Domain.Misc;
using Domain.Users;

namespace Domain.Providers
{
    public class Provider : Audit, IIdentifiableObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Direction { get; set; }
        public long ZipCode { get; set; }
        public string Contact { get; set; }
        public string IiBb { get; set; }
        public string Cuit { get; set; }
        public string Description { get; set; }
        public string NameContact { get; set; }
        public string MailContact { get; set; }
        public string PhoneContact { get; set; }
        public virtual Ubication Ubication { get; set; }
        public virtual IList<PedidosProducto> PedidosProductos { get; set; }
        public virtual TaxCondition TaxCondition { get; set; }

    }
}
