using System.Collections.Generic;
using BassinoLibrary.Resource;
using System.ComponentModel.DataAnnotations;
using BassinoLibrary.Attributes;
using Domain.Almacen;
using Domain.IdentificableObject;

namespace BassinoLibrary.ViewModels
{ 
    public class StockViewModel :IIdentifiableObject
    {
		public int Id { get; set; }
        public int? Secuencia { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Description")]
        [TextCustoml]
        public string Descripcion { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "NombreAlmacen")]
        
        public string Almacen { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "NombreAlmacen")]
        public int AlmacenId { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "NombreProducto")]
        
        public string Producto { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "NombreProducto")]
        
        public int  ProductoId { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Cantidad")]
        [CantidadCustom]
        public int Cantidad { get; set; }

        public IEnumerable<MovimientosStockViewModel> Tracks { get; set; }
	}
}