using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.IdentificableObject;
using Domain.Users;

namespace Domain.Products
{
    public class Promociones : Audit, IIdentifiableObject
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public DateTime DateTimeInicio { get; set; }
        public DateTime DateTimeFin { get; set; }
        public decimal Porcentage { get; set; }
        public bool Activada { get; set; }
        public bool Todos { get; set; }
    }
}
