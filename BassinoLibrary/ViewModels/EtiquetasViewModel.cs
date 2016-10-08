using BassinoLibrary.Resource;
using System.ComponentModel.DataAnnotations;
using Domain.IdentificableObject;

namespace BassinoLibrary.ViewModels
{ 
    public class EtiquetasViewModel :IIdentifiableObject
    {
		public int Id { get; set; }
        public int? Secuencia { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Description")]
        public string Descripcion { get; set; }
	}
}