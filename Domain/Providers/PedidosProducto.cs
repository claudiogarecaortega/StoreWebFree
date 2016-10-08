using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contable;
using Domain.IdentificableObject;
using Domain.Products;
using Domain.Users;

namespace Domain.Providers
{
    public class PedidosProducto : Audit, IIdentifiableObject
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string Referencia { get; set; }
        public decimal Precio { get; set; }
        public decimal Adelantado { get; set; }
        public bool Pagado { get; set; }
        public DateTime FechaDePago { get; set; }
        public virtual Provider Provedor { get; set; }
        public virtual CuentasPagar CuentasPagar { get; set; }
        public virtual IList<Product> Productos  { get; set; }
        public virtual IList<Imagenes> Imagenes { get; set; }
    }
}
