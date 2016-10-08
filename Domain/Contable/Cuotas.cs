using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.IdentificableObject;
using Domain.Users;

namespace Domain.Contable
{
    public class Cuotas : Audit, IIdentifiableObject
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public decimal Valor { get; set; }
        public DateTime Vencimiento { get; set; }
        public bool Pagada { get; set; }
        public decimal Interes { get; set; }
        public virtual Credito Credito { get; set; }
        public string Status()
        {
           
                if (Pagada)
                    return "green";
                if (IsDelete)
                    return "red";


                return "yellow";
        }

    }
}
