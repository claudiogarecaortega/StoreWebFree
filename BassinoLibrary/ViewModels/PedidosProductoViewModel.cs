using System;
using BassinoLibrary.Resource;
using System.ComponentModel.DataAnnotations;
using BassinoLibrary.Attributes;
using Domain.IdentificableObject;

namespace BassinoLibrary.ViewModels
{ 
    public class PedidosProductoViewModel :IIdentifiableObject
    {
		public int Id { get; set; }
		public int? Secuencia { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Description")]
        public string Descripcion { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Referencia")]
        public string Referencia { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Precio")]
        [DecimaCustoml]
        [DecimalPriceCustom]
        public decimal Precio { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Adelanto")]
        public decimal Adelantado { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Pagado")]
        public bool Pagado { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Provider")]
        public string Provedor { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Provider")]
        public int ProvedorId { get; set; }
        public int[] Productos { get; set; }

        public string Productosdescripcion { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "FechaPago")]
        public string FechaDePago { get; set; }
        }
}