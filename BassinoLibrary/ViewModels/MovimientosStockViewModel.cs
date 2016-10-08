using BassinoLibrary.Resource;
using System.ComponentModel.DataAnnotations;
using BassinoLibrary.Attributes;
using Domain.Almacen;
using Domain.IdentificableObject;

namespace BassinoLibrary.ViewModels
{ 
    public class MovimientosStockViewModel :IIdentifiableObject
    {
		public int Id { get; set; }
        public int? Secuencia { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Description")]
        public string Descripcion { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "NombreAlmacen")]
        public string Stock { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "NombreProducto")]
        public string StockProducto { get; set; }
        public int  StockId { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Cantidad")]
        [CantidadCustom]
        public int Cantidad { get; set; }
        public bool EsIngreso { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Provider")]
        public string Proveedor { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Provider")]
        public int ProveedorId { get; set; }

        public string DateCreate { get; set; }
        public string UserCreate { get; set; }
	}
}