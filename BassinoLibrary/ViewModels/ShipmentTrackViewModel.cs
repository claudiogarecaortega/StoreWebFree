using System;
using BassinoLibrary.Resource;
using System.ComponentModel.DataAnnotations;
using Domain.IdentificableObject;

namespace BassinoLibrary.ViewModels
{
    public class ShipmentTrackViewModel : IIdentifiableObject
    {
		public int Id { get; set; }
		public int ShipmentId { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Description")]
        public string Observaciones { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Ubication")]
        public string Ubication { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Ubication")]
        public int? UbicationId { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "DateTracking")]
        public string DateTrack { get; set; }
        public bool IsReceived { get; set; }
        public bool IsSended { get; set; }
        public bool IsWarning { get; set; }
        public int CargaId { get; set; }
        public int? Secuencia { get; set; }
        //public ShipmentViewModel ShioModel { get; set; }
	}
}