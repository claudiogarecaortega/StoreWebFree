using BassinoLibrary.Resource;
using System.ComponentModel.DataAnnotations;
using Domain.IdentificableObject;

namespace BassinoLibrary.ViewModels
{
    public class UbicationViewModel : IIdentifiableObject
    {
		public int Id { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Description")]
        public string Description { get; set; }
        public decimal Code { get; set; }
        public string Observations { get; set; }
        public string DescriptionCompleta { get; set; }
        public int? Secuencia { get; set; }

        public int? UbicacionPadreId { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "UbicationParent")]
        public string UbicacionPadre { get; set; }
	}
}