using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.IdentificableObject;
using Domain.Misc;
using Domain.Users;

namespace Domain.Almacen
{
    public class Almacen :Audit, IIdentifiableObject
    {
        public int  Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Calle { get; set; }
        public string Numero { get; set; }
        public string CodigoPostal { get; set; }
        public virtual Ubication Ubicacion { get; set; }
        public virtual IList<Stock> Stock { get; set; }
    }
}
