using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.IdentificableObject;
using Domain.Products;
using Domain.Users;

namespace Domain.Ventas
{
    public class Pedidos : Audit, IIdentifiableObject
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public bool Cumplido { get; set; }
        public string Referencia { get; set; }
        public virtual UserExtended Usuario { get; set; }
        public virtual  IList<Imagenes>Imagenes  { get; set; }
    }
}
