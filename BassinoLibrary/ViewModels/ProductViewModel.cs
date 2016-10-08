using System;
using BassinoLibrary.Resource;
using System.ComponentModel.DataAnnotations;
using BassinoLibrary.Attributes;
using Domain.IdentificableObject;

namespace BassinoLibrary.ViewModels
{
    public class ProductViewModel : IIdentifiableObject
    {
		public int Id { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Description")]
        [TextCustoml]
        
        public string Description { get; set; }

        public string DescriptionCold { get; set; }
        public int? Secuencia { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "NombreProducto")]
        
        [TextCustoml]
        public string Nombre { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Precio")]
        [DecimaCustoml]
        public decimal Precio { get; set; }
        
        [Display(ResourceType = typeof(Resources), Name = "Disponible")]
        public bool IsAvailable { get; set; }
        
        [Display(ResourceType = typeof(Resources), Name = "Principal")]
        public bool IsPrincipal { get; set; }
        //[Display(ResourceType = typeof(Resources), Name = "Code")]
        //public string Code { get; set; }
        //[Display(ResourceType = typeof(Resources), Name = "Price")]
        //public decimal Price { get; set; }
        //[Display(ResourceType = typeof(Resources), Name = "MeasureUnit")]
        //public int? MeasureUnitId { get; set; }
        //[Display(ResourceType = typeof(Resources), Name = "MeasureUnit")]
        //public string MeasureUnit { get; set; }
        //[Display(ResourceType = typeof(Resources), Name = "Provider")]
        //public int ProviderId { get; set; }
        //[Display(ResourceType = typeof(Resources), Name = "Provider")]
        //public string Provider { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "IsCold")]
        public bool IsCold { get; set; }

        public int[] CategoriasId { get; set; }
        public string Categorias { get; set; }
        public int[] EtiquetasId { get; set; }
        public string Etiquetas { get; set; }
    }
}