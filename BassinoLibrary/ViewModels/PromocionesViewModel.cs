using BassinoLibrary.Resource;
using System.ComponentModel.DataAnnotations;
using Domain.IdentificableObject;

namespace BassinoLibrary.ViewModels
{ 
    public class PromocionesViewModel :IIdentifiableObject
    {
		public int Id { get; set; }
        public int? Secuencia { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Description")]
        public string Description { get; set; }
	}
}