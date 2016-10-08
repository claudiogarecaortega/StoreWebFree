using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Domain.Almacen;
using Domain.Contable;
using Domain.IdentificableObject;
using Domain.Misc;
using Domain.Providers;
using Domain.Users;
using Domain.Ventas;

namespace Domain.Products
{
    public class Product : Audit, IIdentifiableObject
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Description { get; set; }
        public decimal Precio { get; set; }
        public bool IsCold { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsPrincipal { get; set; }
        public virtual IList<PedidosProducto> Pedidos { get; set; }
        public virtual IList<Stock> Stock { get; set; }
        public virtual IList<Promociones> Promociones { get; set; }
        public virtual IList<Imagenes> Imagenes { get; set; }
        public virtual IList<Comentarios> Comentarios { get; set; }
        public virtual IList<Preguntas> Preguntas { get; set; }
        public virtual IList<Categoria> Categorias { get; set; }
        public virtual IList<Etiquetas> Etiquetas { get; set; }
        public virtual IList<Venta> Ventas { get; set; }
        public virtual IList<Credito> Creditos { get; set; }
        

        public string GetCategoriasString()
        {
            return Categorias.Aggregate("", (current, item) => current + " - " + item.Descripcion);
        }
        public string GetEtiquetasString()
        {
            return Etiquetas.Aggregate("", (current, item) => current + " - " + item.Descripcion);
        }
        //public string Code { get; set; }
        //public decimal Price { get; set; }
        //public virtual MeasureUnit MeasureUnit { get; set; }
        //public virtual Provider Provider { get; set; }

    }
}
