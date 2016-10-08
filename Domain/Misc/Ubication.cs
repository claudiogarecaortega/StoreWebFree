using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.IdentificableObject;
using Domain.Products;
using Domain.Providers;
using Domain.Users;

namespace Domain.Misc
{
    public class Ubication : Audit, IIdentifiableObject
    {

        public int Id { get; set; }
        public decimal Code { get; set; }
        public string Description { get; set; }
        public string Observations { get; set; }
        public virtual Ubication UbicationParent { get; set; }
        public virtual IList<Provider> UserExtendeds { get; set; }
        public virtual IList<Shipment> Shipments { get; set; }


        public string DescripcionCompleta()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(Description);

            var ubicacionPadre = UbicationParent;

            while (ubicacionPadre != null)
            {
                stringBuilder.AppendFormat(" - {0}", ubicacionPadre.Description);

                ubicacionPadre = ubicacionPadre.UbicationParent;
            }

            return stringBuilder.ToString();
        }
    }
}
