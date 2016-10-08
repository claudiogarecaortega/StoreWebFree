using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Clients;
using Domain.Contable;
using Domain.IdentificableObject;
using Domain.Products;
using Domain.Users;

namespace Domain.Ventas
{
    public class Venta : Audit, IIdentifiableObject
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
        public decimal MontoTotal { get; set; }
        public bool Pagada { get; set; }
        public virtual IList<Product> Producto { get; set; }
        public virtual Client Cliente { get; set; }
    }
}
