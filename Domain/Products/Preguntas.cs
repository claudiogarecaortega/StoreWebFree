using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.IdentificableObject;
using Domain.Users;

namespace Domain.Products
{
    public class Preguntas : Audit, IIdentifiableObject
    {
        public int Id { get; set; }
        public string Pregunta { get; set; }
        public bool IsApproved { get; set; }
        public virtual UserExtended Usuario { get; set; }
        public virtual Product Producto { get; set; }
    }
}
