using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.IdentificableObject;
using Domain.Products;
using Domain.Users;

namespace Domain.Misc
{
    public class Categoria : Audit, IIdentifiableObject
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public virtual IList<Product> Productos { get; set; }
    }
}
