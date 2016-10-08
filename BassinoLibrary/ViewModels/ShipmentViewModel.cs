using System.Collections.Generic;
using BassinoLibrary.Resource;
using System.ComponentModel.DataAnnotations;
using Domain.IdentificableObject;
using Domain.Products;

namespace BassinoLibrary.ViewModels
{ 
    public class ShipmentViewModel : IIdentifiableObject
    {
		public int Id { get; set; }
        public string Statuts { get; set; }
        public int? Secuencia { get; set; }
       [Display(ResourceType = typeof(Resources), Name = "TotalKilos")]
        public string TotalKilos { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "TotalPAckages")]
        public string TotalPakages { get; set; }

        public int[] Cargars { get; set; }
        public int[] UbicationRoute { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "UbicationFrom")]
        public string UbicationFrom { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "UbicationFrom")]
        public int? UbicationFromId { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "UbicationTo")]
        public string UbicationTo { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "UbicationTo")]
        public int? UbicationToId { get; set; }
        public string Car { get; set; }
        public int? CarId { get; set; }

        public bool IsSent { get; set; }
        public bool IsFinishig { get; set; }
        public bool IsTraveling { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Observations")]
        public string Observations { get; set; }
        
        public IEnumerable<ShipmentTrackViewModel> Tracks { get; set; }
	}
}