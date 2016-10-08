using BassinoLibrary.Resource;
using System.ComponentModel.DataAnnotations;
using Domain.IdentificableObject;

namespace BassinoLibrary.ViewModels
{ 
    public class CarViewModel :IIdentifiableObject
    {
		public int Id { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Description")]
        public string Description { get; set; }
        public int? Secuencia { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Nombre")]
        public string Nombre { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Patente")]
        public string Patente { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "UsoFisico")]
        public string UsoFisico { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Senasa")]
        public string Senasa { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Senada")]
        public string Senada { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Seguro")]
        public string Seguro { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Poliza")]
        public string Poliza { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "DueDatePoliza")]
        public string DueDate { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "KilosMaximo")]
        public string KiloMaximo { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "PalletMaximo")]
        public string PalletMaximo { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "PhoneUrgenci")]
        public string UrgenciPhone { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "CarPhone")]
        public string CarPhone { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Itv")]
        public string Itv { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "DueItv")]
        public string DueDateItv { get; set; }

        //public bool IsTraveling { get; set; }
       // public bool IsMaintenace { get; set; }
	}
}