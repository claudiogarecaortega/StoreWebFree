using BassinoLibrary.Resource;
using System.ComponentModel.DataAnnotations;
using BassinoLibrary.Attributes;
using Domain.IdentificableObject;

namespace BassinoLibrary.ViewModels
{ 
    public class AlmacenViewModel :IIdentifiableObject
    {
		public int Id { get; set; }
        public int? Secuencia { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Description")]
        [TextCustoml]
        public string Descripcion { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "NombreAlmacen")]
        [TextCustoml]
        public string Nombre { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Calle")]
        [TextCustoml]
        public string Calle { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Numero")]

        public string Numero { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "CodigoPostal")]
        [CodigoPostalCustom]
        public string CodigoPostal { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Ubicacion")]
        public string Ubicacion { get; set; }
        public int? UbicacionId { get; set; }
	}
}