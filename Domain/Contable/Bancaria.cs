using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.IdentificableObject;
using Domain.Users;

namespace Domain.Contable
{
    public class Bancaria : Audit, IIdentifiableObject
    {
        public int Id { get; set; }
        public string NumeroCuenta { get; set; }
        public string Alias  { get; set; }
        public string Entidad { get; set; }
        public decimal Saldo { get; set; }
        public bool Principal { get; set; }
        public bool Ingresos { get; set; }
        public bool Egresos { get; set; }
        
    }
}
