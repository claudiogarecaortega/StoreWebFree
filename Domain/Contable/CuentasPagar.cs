using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.IdentificableObject;
using Domain.Providers;
using Domain.Users;

namespace Domain.Contable
{
    public class CuentasPagar : Audit, IIdentifiableObject
    {
        public int Id { get; set; }
        public string Comentarios { get; set; }
        public decimal Monto { get; set; }
        public bool Pagado { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public virtual Bancaria CuentaExtracto { get; set; }
        public virtual PedidosProducto Credito { get; set; }
    }
}
