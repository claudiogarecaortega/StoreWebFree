using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.IdentificableObject;
using Domain.Users;

namespace Domain.Contable
{
    public class CuentasCobrar : Audit, IIdentifiableObject
    {
        public int Id { get; set; }
        public string Comentarios { get; set; }
        public decimal Monto { get; set; }
        public bool Cobrado { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public virtual  Bancaria CuentaDeposito { get; set; }
        public virtual Credito Credito { get; set; }
        
    }
}
