using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.IdentificableObject;
using Domain.Providers;
using Domain.Users;

namespace Domain.Almacen
{
    public class MovimientosStock : Audit, IIdentifiableObject
    {
        public int Id { get; set; }
        public virtual Stock Stock { get; set; }
        public virtual Provider Proveedor { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public bool EsIngreso { get; set; }
        public bool Pedido { get; set; }
    }
}
