using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.IdentificableObject;
using Domain.Users;

namespace Domain.Products
{
    public class Imagenes : Audit, IIdentifiableObject
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public byte[] FileContent { get; set; }
    }
}
