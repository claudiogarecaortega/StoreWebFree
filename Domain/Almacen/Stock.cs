using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.IdentificableObject;
using Domain.Products;
using Domain.Users;

namespace Domain.Almacen
{
    public class Stock : Audit, IIdentifiableObject
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public virtual Almacen Almacen { get; set; }
        public virtual Product Producto { get; set; }
        public int Cantidad { get; set; }
        public virtual IList<MovimientosStock> Movimientos { get; set; }
    }
}
